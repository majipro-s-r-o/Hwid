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
                .ConfigureServices((_, services) =>
                {
                    services.AddHwid();
                })
                .Build();

            var hwidAccessor = host.Services.GetRequiredService<IHwidAccessorService>();

            Console.WriteLine($"Running on '{RuntimeInformation.OSDescription}'");
            
            string hwidAsync = await hwidAccessor.GetHwidAsync();
            Console.WriteLine($"HWID produced by '{nameof(hwidAccessor.GetHwidAsync)}': '{hwidAsync}'");
            
            string hwid = hwidAccessor.GetHwid();
            Console.WriteLine($"HWID produced by '{nameof(hwidAccessor.GetHwid)}': '{hwid}'");

            if (hwidAsync == hwid && hwidAsync?.Length > 5 && hwid?.Length > 5)
            {
                Console.WriteLine("Test passed.");
            }
            else
            {
                await Console.Error.WriteLineAsync("HWID is invalid.");
            }
        }
    }
}