using System;

namespace TogglSnusProxy.Toggl {
  public class TimeEntry {
    public int id { get; set; }
    public int pid { get; set; }
    public bool billable { get; set; }
    public DateTime start { get; set; }
    public DateTime stop { get; set; }
    public string description { get; set; }

    public override string ToString()
      => $"id: {id}, {description}";

    public bool IsLogging()
      => stop == null || stop == DateTime.MinValue;
  }
}
