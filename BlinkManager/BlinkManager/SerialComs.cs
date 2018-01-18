using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace BlinkManager {

  public class SerialComs {

    private static DateTime lastWrite;

    public SerialDevice Device { get; set; }

    public bool IsInitialized => Device != null;

    public async Task InitSerial() {

      var aqs = SerialDevice.GetDeviceSelector();
      var dis = await DeviceInformation.FindAllAsync(aqs);
      var dev = dis.FirstOrDefault();

      var serialDevice = await SerialDevice.FromIdAsync(dev.Id);

      if (serialDevice == null) return;

      // Configure serial settings
      var settings = new SerialSettings();
      settings.Set(serialDevice);

      Device = serialDevice;
    }

    public async Task WriteSerial(string input) {

      if (DateTime.Now - lastWrite < TimeSpan.FromMilliseconds(20))
        return;

      lastWrite = DateTime.Now;

      if (!IsInitialized)
        await InitSerial();

      var dataWriteObject = new DataWriter(Device.OutputStream);
      dataWriteObject.WriteString(input);
      var bytesWritten = await dataWriteObject.StoreAsync();
    }
  }
}
