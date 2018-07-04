using System;

namespace TogglSnusProxy.Util {
  internal static class Logger {
    public static void Log(string log)
      => Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} - {log}");
  }
}
