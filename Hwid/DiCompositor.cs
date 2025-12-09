using System;
using Majipro.Hwid.Services.HwidAccessor;
using Majipro.Hwid.Services.HwidProvision;
using Majipro.Hwid.Services.ProcessLauncher;
using Microsoft.Extensions.DependencyInjection;

namespace Majipro.Hwid;

public static class DiCompositor
{
    public static IServiceCollection AddHwid(this IServiceCollection serviceCollection, params IHwidProvisionService[] provisionServices)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }
        
        serviceCollection.AddSingleton<IHwidProvisionService, OsXHwidProvisionService>();
        serviceCollection.AddSingleton<IHwidProvisionService, WindowsHwidProvisionService>();

        foreach (var hwidProvisionService in provisionServices)
        {
            serviceCollection.AddSingleton(typeof(IHwidProvisionService), hwidProvisionService.GetType());
        }
        
        serviceCollection.AddSingleton<IProcessLauncherService, ProcessLauncherService>();
        serviceCollection.AddSingleton<IHwidAccessorService, HwidAccessorService>();
        
        return serviceCollection;
    }
}