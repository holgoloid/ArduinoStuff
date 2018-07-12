using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TogglSnusProxy {
  public class SnusService : IHostedService {
    private SnusProxy _proxy;

    public Task StartAsync(CancellationToken cancellationToken) {
      _proxy = new SnusProxy();
      _proxy.Run();
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
      return Task.CompletedTask;
    }
  }
}
