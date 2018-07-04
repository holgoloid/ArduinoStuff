namespace TogglSnusProxy.Snus {
  public class MqttResponse {
    public bool isLogging { get; set; }
    public Color color { get; set; }
  }

  public class Color {
    public int r { get; set; }
    public int g { get; set; }
    public int b { get; set; }
  }
}
