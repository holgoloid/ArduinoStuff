using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TogglSnusProxy.Toggl;

namespace TogglSnusProxy {

  class Program {

    private static MqttHandler mqtt = new MqttHandler();

    private static List<TogglUser> Users { get; set; } = new List<TogglUser>();

    static async Task Main(string[] args) {

      //Users.Add(new TogglUser { Name = "fredrik", Token = "04d9e8f7acba22d60f13891e7943c0bc"});
      Users.Add(new TogglUser { Name = "marcus", Token = "d8c314c8a1705306eb077669b9e30fcf" });

      mqtt.Initialize();

      mqtt.EventRecieved += async (e) => {

        var user = Users.FirstOrDefault(x => x.Name == e.user);

        if (string.IsNullOrEmpty(user?.Token)) {
          Console.WriteLine("Okänd användare: " + e.user);
          return;
        }

        if (e.@event == "click")
          await HandleClick(user);
        else if (e.@event == "hold")
          await HandleHold(user);
        else if (e.@event == "doubleclick")
          await HandleDoubleClick(user);
      };

      while (true) {
        Thread.Sleep(120_000);

        foreach (var user in Users) {
          var toggl = new TogglApi(user.Token);
          var entry = await toggl.GetCurrentTimeEntry();
          var isLogging = entry != null;
          mqtt.ReportStatus(user.Name, isLogging);
        }
      }
    }

    private static async Task HandleDoubleClick(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      await toggl.StartNewTimeEntry();
      mqtt.ReportStatus(user.Name, true);
    }

    private static async Task HandleHold(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      var entries = await toggl.GetEntries();

      var secondLastEntry = entries?.OrderByDescending(x => x.start).Skip(1).FirstOrDefault();
      if (secondLastEntry != null)
        await toggl.ContinueLogging(secondLastEntry);
      else
        await toggl.StartNewTimeEntry();

      mqtt.ReportStatus(user.Name, true);
    }

    private static async Task HandleClick(TogglUser user) {
      var toggl = new TogglApi(user.Token);
      var entries = await toggl.GetEntries();

      var isLogging = false;

      if (entries?.Any(x => x.IsLogging()) ?? false) {

        foreach (var entry in entries?.Where(x => x.IsLogging()))
          await toggl.StopLogging(entry);

      } else {

        var lastEntry = entries?.OrderByDescending(x => x.start).FirstOrDefault();
        if (lastEntry != null)
          await toggl.ContinueLogging(lastEntry);
        else
          await toggl.StartNewTimeEntry();

        isLogging = true;
      }

      mqtt.ReportStatus(user.Name, isLogging);
    }
  }
}
