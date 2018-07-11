using System;
using System.Runtime.CompilerServices;

namespace TogglSnusProxy.Util {
  internal static class Logger {
    public static void Log(string log)
      => Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} - {log}");


    public static void LogHttpResponse(int code, [CallerMemberName] string caller = "")
      => Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} - {caller} - {code}");
  }
}
