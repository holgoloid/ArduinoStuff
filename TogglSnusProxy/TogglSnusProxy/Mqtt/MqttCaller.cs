using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TogglSnusProxy.Snus;
using TogglSnusProxy.Util;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TogglSnusProxy {
  public class MqttHandler {

    private const string brokerUrl = "broker.hivemq.com";
    private static string publisher = "togglProxy" + Environment.MachineName;

    public const string baseTopic = "easyfy/toggl";

    public event Func<MqttEvent, Task> EventRecieved;

    private MqttClient client;

    public bool IsConnected => client.IsConnected;

    public void Connect() {

      Logger.Log($"Kontaktar {brokerUrl}");

      client = new MqttClient(brokerUrl);

      client.Connect(publisher);
      client.Publish($"{baseTopic}/control", Encoding.UTF8.GetBytes(publisher + " connected"));

      var topic = $"{baseTopic}/toproxy";
      client.Subscribe(new[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

      Logger.Log($"Lyssnar på {topic}");

      client.MqttMsgPublishReceived += (o, e) => {
        var message = Encoding.UTF8.GetString(e.Message);
        Logger.Log($"{e.Topic}: {message}");

        var parsed = JsonConvert.DeserializeObject<MqttEvent>(message);

        EventRecieved.Invoke(parsed);
      };

      client.ConnectionClosed += (o, e) => Connect();
    }

    public void Disconnect() => client.Disconnect();

    public void ReportStatus(string apiToken, bool isLogging, Color color = null) {
      var response = new MqttResponse {
        isLogging = isLogging,
        color = color ?? new Color()
      };
      Logger.Log(response.ToString());
      var json = JsonConvert.SerializeObject(response);
      client.Publish($"{baseTopic}/fromproxy/{apiToken}", Encoding.UTF8.GetBytes(json));
    }
  }
}
