using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Majipro.Hwid.Services.HwidProvision;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Majipro.Hwid.Tests;

public abstract class ProvisionServiceTestsBase
{
    internal IHwidProvisionService HwidProvisionService { get; private set; }
    internal IServiceProvider ServiceProvider { get; private set;}

    internal async Task TestInitializeAsync<TService>(Action<IServiceCollection> configureServices = null)
        where TService : IHwidProvisionService
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddHwid();

                if (configureServices != null)
                {
                    configureServices(services);
                }
            })
            .Build();

        HwidProvisionService = host.Services
            .GetRequiredService<IEnumerable<IHwidProvisionService>>()
            .OfType<TService>()
            .Single();

        ServiceProvider = host.Services;
        
        await host.StartAsync();
    }
}