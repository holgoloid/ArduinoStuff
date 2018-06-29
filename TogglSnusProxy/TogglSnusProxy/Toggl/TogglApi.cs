using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace TogglSnusProxy.Toggl {
  public class TogglApi {

    private string GetAuth()
      => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiToken}:api_token"));

    private const string api = "https://www.toggl.com/api/v8/time_entries";

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

      Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

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

      Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

      return true;
    }

    public async Task<TimeEntry> GetCurrentTimeEntry() {

      var response = await GetClient().GetAsync($"{api}/current");

      Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

      var content = response.Content;
      var result = await content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TimeEntryRoot>(result).data;
    }

    public async Task<TimeEntry> GetLastTimeEntry() {
      var entries = await GetEntries();
      return entries?.OrderByDescending(x => x.start).FirstOrDefault();
    }

    public async Task<bool> StopLogging(TimeEntry entry) {
      var response = await GetClient().PutAsync($"{api}/{entry.id}/stop", null);
      Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);
      return true;
    }

    public async Task<TimeEntry[]> GetEntries() {
      var response = await GetClient().GetAsync($"{api}");

      Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

      var content = response.Content;
      var result = await content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<TimeEntry[]>(result);
    }
  }
}
