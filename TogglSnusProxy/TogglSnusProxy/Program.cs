using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TogglSnusProxy {

  class Program {
    static async Task Main(string[] args) {

      var hostBuilder = new HostBuilder().ConfigureServices((hostContext, services) => {
        services.AddSingleton<IHostedService, SnusService>();
      });

      await hostBuilder.RunConsoleAsync();
    }
  }
}
