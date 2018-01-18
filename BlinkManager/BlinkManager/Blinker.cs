using System;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BlinkManager {
  public class Blinker {

    private SerialComs coms { get; set; } = new SerialComs();

    public bool IsInitialized => coms.IsInitialized;
    
    public async Task Init() => await coms.InitSerial();

    public async Task WriteBlink(Blink blink)
      => await coms.WriteSerial(blink.ToSerial());

    public async Task SyncGlow()
      => await coms.WriteSerial($"99{Environment.NewLine}");
  }
}
