using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Majipro.Hwid.Demo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                    });
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHwid();
                })
                .Build();

            var hwidAccessor = host.Services.GetRequiredService<IHwidAccessorService>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            using (logger.BeginScope(RuntimeInformation.OSDescription))
            {
                string hwid1 = await hwidAccessor.GetHwidAsync();
                logger.LogInformation("Result of '{MethodCall}': '{Hwid}'", nameof(hwidAccessor.GetHwidAsync), hwid1);
                
                string hwid2 = await hwidAccessor.GetHwidAsync();
                logger.LogInformation("Result of '{MethodCall}': '{Hwid}'", nameof(hwidAccessor.GetHwid), hwid2);
            }
        }
    }
}