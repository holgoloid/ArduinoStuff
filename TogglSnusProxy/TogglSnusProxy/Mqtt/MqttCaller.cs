using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TogglSnusProxy.Snus;
using TogglSnusProxy.Toggl;
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

    public void Initialize() {

      client = new MqttClient(brokerUrl);

      client.Connect(publisher);
      client.Publish($"{baseTopic}/control", Encoding.UTF8.GetBytes(publisher + " connected"));

      client.Subscribe(new[] { $"{baseTopic}/toproxy" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

      client.MqttMsgPublishReceived += (o, e) => {
        var message = Encoding.UTF8.GetString(e.Message);
        Logger.Log($"{e.Topic}: {message}");

        var parsed = JsonConvert.DeserializeObject<MqttEvent>(message);

        EventRecieved.Invoke(parsed);
      };
    }

    public void ReportStatus(string apiToken, bool isLogging, Color color = null) {
      var answer = JsonConvert.SerializeObject(
        new MqttResponse { 
        isLogging = isLogging,
        color = color
      });
      client.Publish($"{baseTopic}/fromproxy/{apiToken}", Encoding.UTF8.GetBytes(answer));
    }
  }
}
