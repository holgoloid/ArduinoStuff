using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using TogglSnusProxy.Util;

namespace TogglSnusProxy.Toggl {
  public class TogglApi {

    private string GetAuth()
      => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiToken}:api_token"));

    private const string api = "https://www.toggl.com/api/v8/time_entries";
    private const string projectsApi = "https://www.toggl.com/api/v8/projects";

    private string apiToken { get; set; }

    public TogglApi (string _apiToken) {
      apiToken = _apiToken;
    }

    private HttpClient GetClient() {
      var http = new HttpClient();
      http.DefaultRequestHeaders.Accept.Clear();
      http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetAuth());
      return http;
    }

    public async Task<bool> ContinueLogging(TimeEntry entry) {

      var response = await GetClient().PostAsync(
        $"{api}/start",
        new StringContent(
          JsonConvert.SerializeObject(new {
            time_entry = new {
              created_with = "easyProxy",
              entry.description,
              entry.pid,
              entry.billable
            }}))
      );

      Logger.LogHttpResponse((int)response.StatusCode);

      return true;
    }


    public async Task<bool> StartNewTimeEntry() {

      var response = await GetClient().PostAsync(
        $"{api}/start",
        new StringContent(
          JsonConvert.SerializeObject(new {
            time_entry = new {
              created_with = "easyProxy",
              description = ""
            }})));

      Logger.LogHttpResponse((int)response.StatusCode);

      return true;
    }

    public async Task<TimeEntry> GetCurrentTimeEntry() {

      var response = await GetClient().GetAsync($"{api}/current");

      Logger.LogHttpResponse((int)response.StatusCode);

      var content = response.Content;
      var result = await content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TogglRoot<TimeEntry>>(result).data;
    }

    public async Task<Project> GetProject(int projectId) {

      var response = await GetClient().GetAsync($"{projectsApi}/{projectId}");

      Logger.LogHttpResponse((int)response.StatusCode);

      var content = response.Content;
      var result = await content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TogglRoot<Project>>(result).data;
    }

    public async Task<TimeEntry> GetLastTimeEntry() {
      var entries = await GetEntries();
      return entries?.OrderByDescending(x => x.start).FirstOrDefault();
    }

    public async Task<bool> StopLogging(TimeEntry entry) {
      var response = await GetClient().PutAsync($"{api}/{entry.id}/stop", null);
      Logger.LogHttpResponse((int)response.StatusCode);
      return true;
    }

    public async Task<TimeEntry[]> GetEntries() {
      var response = await GetClient().GetAsync($"{api}");

      Logger.LogHttpResponse((int)response.StatusCode);

      var content = response.Content;
      var result = await content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TimeEntry[]>(result);
    }
  }
}
