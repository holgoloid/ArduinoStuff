
namespace BlinkManager {
  public static class BlinkBuilder {

    public static Blink BaseColor(this Blink blink) => blink.SetCommand(1);
    public static Blink Glow(this Blink blink) => blink.SetCommand(2);
    public static Blink Blink(this Blink blink) => blink.SetCommand(3);
    public static Blink SetCommand(this Blink blink, int command) {
      blink.Command = command;
      return blink;
    }

    public static Blink Quick(this Blink blink) => blink.SetSpeed(2);
    public static Blink MediumSpeed(this Blink blink) => blink.SetSpeed(15);
    public static Blink Slow(this Blink blink) => blink.SetSpeed(25);
    public static Blink SetSpeed(this Blink blink, int speed) {
      blink.Speed = speed;
      return blink;
    }

    public static Blink Short(this Blink blink) => blink.SetDuration(50);
    public static Blink MediumLength(this Blink blink) => blink.SetDuration(100);
    public static Blink Long(this Blink blink) => blink.SetDuration(150);
    public static Blink SetDuration(this Blink blink, int duration) {
      blink.Duration = duration;
      return blink;
    }

    const int full = 200;
    public static Blink Red(this Blink blink) => blink.Color(full, 0, 0);
    public static Blink Green(this Blink blink) => blink.Color(0, full, 0);
    public static Blink Blue(this Blink blink) => blink.Color(0, 0, full);
    public static Blink Cyan(this Blink blink) => blink.Color(0, full, full);
    public static Blink Yellow(this Blink blink) => blink.Color(full, full, 0);
    public static Blink Violet(this Blink blink) => blink.Color(full, 0, full);
    public static Blink White(this Blink blink) => blink.Color(full, full, full);
    public static Blink Color(this Blink blink, int r = 0, int g = 0, int b = 0) {
      blink.R = r;
      blink.G = g;
      blink.B = b;
      return blink;
    }

    public static Blink Pos(int lightIndex = 1) => new Blink(lightIndex);
  }
}
