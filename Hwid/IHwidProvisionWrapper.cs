namespace Majipro.Hwid;

public interface IHwidProvisionWrapper
{
    /// <summary>
    /// Register custom provision service.
    /// </summary>
    /// <typeparam name="TService">The custom provision service that implements <see cref="IHwidProvisionService"/>.</typeparam>
    /// <returns>The <see cref="IHwidProvisionService"/>.</returns>
    IHwidProvisionWrapper AddProvisionService<TService>() where TService : class, IHwidProvisionService;
}