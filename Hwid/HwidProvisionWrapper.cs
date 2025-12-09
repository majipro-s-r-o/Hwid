using Microsoft.Extensions.DependencyInjection;

namespace Majipro.Hwid;

internal sealed class HwidProvisionWrapper : IHwidProvisionWrapper
{
    private readonly IServiceCollection _serviceCollection;
    
    internal HwidProvisionWrapper(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    
    public IHwidProvisionWrapper AddProvisionService<TService>()
        where TService : class, IHwidProvisionService
    {
        _serviceCollection.AddSingleton<IHwidProvisionService, TService>();
        
        return this;
    }
}