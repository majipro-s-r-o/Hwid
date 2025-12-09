using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Majipro.Hwid.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    //[Ignore]
    public async Task TestMethod1()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddHwid();
            })
            .Build();

        var hwidAccessor = host.Services.GetRequiredService<IHwidAccessorService>();

        string hwid1 = await hwidAccessor.GetHwidAsync();
        string hwid2 = hwidAccessor.GetHwid();
        
        await host.StartAsync();
    }
}