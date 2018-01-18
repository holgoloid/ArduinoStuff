using System;

namespace BlinkManager {
  public class Blink {
    public int LightIndex { get; set; } = 1;
    public int Command { get; set; } = 1;
    public int R { get; set; } = 0;
    public int G { get; set; } = 0;
    public int B { get; set; } = 0;
    public int Speed { get; set; } = 20;
    public int Duration { get; set; } = 100;

    public Blink(int lightIndex) {
      LightIndex = lightIndex;
    }

    public string ToSerial() {
      if (Command == 1)
        return string.Join(",", LightIndex, Command, R, G, B) + Environment.NewLine;
      if (Command == 2)
        return string.Join(",", LightIndex, Command, R, G, B, Duration, Speed) + Environment.NewLine;
      if (Command == 3)
        return string.Join(",", LightIndex, Command, R, G, B, Duration, Speed) + Environment.NewLine;
      return "";
    }
  }
}
