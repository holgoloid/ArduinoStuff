using System;
using Windows.Devices.SerialCommunication;

namespace BlinkManager {
  public class SerialSettings {
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromMilliseconds(100);
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromMilliseconds(100);
    public uint BaudRate { get; set; } =  9600;
    public SerialParity Parity { get; set; } = SerialParity.None;
    public SerialStopBitCount StopBits { get; set; } = SerialStopBitCount.One;
    public ushort DataBits { get; set; } = 8;
    public SerialHandshake Handshake { get; set; } = SerialHandshake.None;

    public void Set(SerialDevice serialDevice) {
      serialDevice.WriteTimeout = WriteTimeout;
      serialDevice.ReadTimeout = ReadTimeout;
      serialDevice.BaudRate = BaudRate;
      serialDevice.Parity = Parity;
      serialDevice.StopBits = StopBits;
      serialDevice.DataBits = DataBits;
      serialDevice.Handshake = Handshake;
    }
  }
}
