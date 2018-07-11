namespace TogglSnusProxy.Snus {
  public class MqttResponse {
    public bool isLogging { get; set; }
    public Color color { get; set; }

    public override string ToString()
      => $"isLogging: {isLogging}, color {color}";
  }

  public class Color {
    public int r { get; set; }
    public int g { get; set; }
    public int b { get; set; }

    public override string ToString()
      => $"r: {r}, g: {g}, b: {b}";
  }
}
