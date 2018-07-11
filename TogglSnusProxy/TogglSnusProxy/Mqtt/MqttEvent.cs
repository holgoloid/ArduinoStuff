namespace TogglSnusProxy {
  public class MqttEvent {
    public string @event { get; set; }
    public string user { get; set; }

    public override string ToString()
      => $"{user}: {@event}";
  }
}
