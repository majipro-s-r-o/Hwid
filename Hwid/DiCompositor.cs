using System;
using Majipro.Hwid.Services.HwidAccessor;
using Majipro.Hwid.Services.HwidOptions;
using Majipro.Hwid.Services.HwidProvision;
using Majipro.Hwid.Services.ProcessLauncher;
using Majipro.Hwid.Services.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Majipro.Hwid;

public static class DiCompositor
{
    /// <summary>
    /// Registers HWID services.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHwid(this IServiceCollection serviceCollection)
    {
        return AddHwid(serviceCollection, null, null);
    }
    
    /// <summary>
    /// Registers HWID services.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureOptions">The <see cref="Action{IHwidOptions}"/> that allows you to configure service.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHwid(this IServiceCollection serviceCollection, Action<IHwidOptions> configureOptions)
    {
        return AddHwid(serviceCollection, configureOptions, null);
    }
    
    /// <summary>
    /// Registers HWID services.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureProvisionServices">The <see cref="Action{IHwidProvisionWrapper}"/> that allows you to register custom provision services.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHwid(this IServiceCollection serviceCollection, Action<IHwidProvisionWrapper> configureProvisionServices)
    {
        return AddHwid(serviceCollection, null, configureProvisionServices);
    }

    /// <summary>
    /// Registers HWID services.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureOptions">The <see cref="Action{IHwidProvisionWrapper}"/> that allows you to register custom provision services.</param>
    /// <param name="configureProvisionServices">The <see cref="Action{IHwidProvisionWrapper}"/> that allows you to register custom provision services.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHwid(this IServiceCollection serviceCollection, Action<IHwidOptions> configureOptions, Action<IHwidProvisionWrapper> configureProvisionServices)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }
        
        var options = new HwidOptionsService();
        
        if (configureOptions != null)
        {
            configureOptions(options);
        }
        
        serviceCollection.AddSingleton<IHwidOptionsService, HwidOptionsService>(_ => options);
        
        serviceCollection.AddSingleton<IHwidProvisionService, LinuxHwidProvisionService>();
        serviceCollection.AddSingleton<IHwidProvisionService, OsXHwidProvisionService>();
        serviceCollection.AddSingleton<IHwidProvisionService, WindowsHwidProvisionService>();

        if (configureProvisionServices != null)
        {
            var wrapper = new HwidProvisionWrapper(serviceCollection);
            
            configureProvisionServices(wrapper);
        }

        serviceCollection.AddSingleton<IProcessLauncherService, ProcessLauncherService>();
        serviceCollection.AddSingleton<IHwidAccessorService, HwidAccessorService>();
        serviceCollection.AddSingleton<IRuntimeService, RuntimeService>();
        
        return serviceCollection;
    }
}