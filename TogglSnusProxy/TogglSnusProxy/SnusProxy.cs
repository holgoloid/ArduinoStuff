using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TogglSnusProxy.Toggl;
using TogglSnusProxy.Util;

namespace TogglSnusProxy {

  public class SnusProxy { 
    private static MqttHandler mqtt = new MqttHandler();

    private static List<TogglUser> Users { get; set; } = new List<TogglUser>();

    private static List<Project> ProjectCache { get; set; } = new List<Project>();

    private static Snus.Color NoProjectColor => new Snus.Color { r = 250, g = 0, b = 0 };

    public void Run() { 
      Logger.Log("Startar upp..");

      Users.Add(new TogglUser { Name = "fredrik", Token = "04d9e8f7acba22d60f13891e7943c0bc"});
      Users.Add(new TogglUser { Name = "marcus", Token = "d8c314c8a1705306eb077669b9e30fcf" });

      Logger.Log($"Användare: {string.Join(", ", Users.Select(x => x.Name))}");
      
      mqtt.EventRecieved += async (e) => {

        var user = Users.FirstOrDefault(x => x.Name == e.user);

        if (string.IsNullOrEmpty(user?.Token)) {
          Logger.Log("Okänd användare: " + e.user);
          return;
        }

        if (e.@event == "click")
          await HandleClick(user);
        else if (e.@event == "hold")
          await HandleHold(user);
        else if (e.@event == "doubleclick")
          await HandleDoubleClick(user);
        else if (e.@event == "refresh")
          await HandleRefresh(user);
      };

      mqtt.Connect(); 
    }

    private static async Task HandleDoubleClick(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      await toggl.StartNewTimeEntry();
      mqtt.ReportStatus(user.Name, true, NoProjectColor);
    }

    private static async Task HandleHold(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      var entries = await toggl.GetEntries();

      var secondLastEntry = entries?.OrderByDescending(x => x.start).Skip(1).FirstOrDefault();
      if (secondLastEntry != null) {
        await toggl.ContinueLogging(secondLastEntry);

        var color = await GetColorForProject(toggl, secondLastEntry.pid);
        mqtt.ReportStatus(user.Name, true, color);

      } else {
        await toggl.StartNewTimeEntry();
        mqtt.ReportStatus(user.Name, true, NoProjectColor);
      }
    }

    private static async Task HandleClick(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      var entries = await toggl.GetEntries();

      if (entries?.Any(x => x.IsLogging()) ?? false) {

        foreach (var entry in entries?.Where(x => x.IsLogging()))
          await toggl.StopLogging(entry);

        mqtt.ReportStatus(user.Name, false);

      } else {

        var lastEntry = entries?.OrderByDescending(x => x.start).FirstOrDefault();
        if (lastEntry != null) {
          await toggl.ContinueLogging(lastEntry);

          var color = await GetColorForProject(toggl, lastEntry.pid);
          mqtt.ReportStatus(user.Name, true, color);

        } else {
          await toggl.StartNewTimeEntry();
          mqtt.ReportStatus(user.Name, true, NoProjectColor);
        }

      }
    }

    private static async Task HandleRefresh(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      var currentEntry = await toggl.GetCurrentTimeEntry();

      if (currentEntry != null) {
        var color = await GetColorForProject(toggl, currentEntry.pid);
        mqtt.ReportStatus(user.Name, true, color);
      }
      else 
        mqtt.ReportStatus(user.Name, false);
    }

    private static async Task<Project> GetProject(TogglApi toggl, int pid) {

      if (pid <= 0)
        return null;

      var project = ProjectCache.FirstOrDefault(x => x.id == pid);

      if (project == null) {
        project = await toggl.GetProject(pid);
        if (project != null)
          ProjectCache.Add(project);
      }

      return project;
    }

    private static async Task<Snus.Color> GetColorForProject(TogglApi toggl, int pid) {
      var project = await GetProject(toggl, pid);
      return project?.GetColor() ?? NoProjectColor;
    }
  }
}
