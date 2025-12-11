using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Majipro.Hwid.Services.ProcessLauncher;
using Majipro.Hwid.Services.Runtime;
using Majipro.Hwid.Tests.Mocking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Majipro.Hwid.Tests.Tests;

[TestClass]
public sealed class HwidAccessorServiceTests
{
    internal IServiceProvider ServiceProvider { get; private set;}

    [TestInitialize]
    public async Task TestInitializeAsync()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddHwid();

                services.AddSingleton<IHwidProvisionService, HwidProvisionMockedService>();
                services.AddSingleton<IProcessLauncherService, ProcessLauncherMockedService>();
                services.AddSingleton<IRuntimeService, RuntimeMockedService>();
            })
            .Build();

        ServiceProvider = host.Services;
        
        await host.StartAsync();
    }
    
    [TestMethod]
    public async Task When_Real_Id_Is_Known_Then_The_Output_Hash_Should_Be_Always_The_Same()
    {
        // Arrange
        var hwidProvisionService = ServiceProvider.GetRequiredService<IHwidProvisionService>() as HwidProvisionMockedService;
        var runtimeService = ServiceProvider.GetRequiredService<IRuntimeService>() as RuntimeMockedService;
        var processLauncherService = ServiceProvider.GetRequiredService<IProcessLauncherService>() as ProcessLauncherMockedService;
        var hwidAccessorService = ServiceProvider.GetRequiredService<IHwidAccessorService>();
        
        Assert.IsNotNull(hwidProvisionService);
        Assert.IsNotNull(runtimeService);
        Assert.IsNotNull(processLauncherService);
        
        processLauncherService.MockedOutput = "TEST-HWID";
        
        hwidProvisionService.BehaveLikePlatform = OSPlatform.Windows;
        
        runtimeService.BehaveLikePlatform = OSPlatform.Windows;

        // Act
        string hwid = hwidAccessorService.GetHwid();
        string hwidAsync = await hwidAccessorService.GetHwidAsync();
        
        // Assert
        const string expectedHwid = "ADA58B4FEA1F99F603712A04983EE84BFB94B72B";
        
        Assert.AreEqual(expectedHwid, hwid);
        Assert.AreEqual(expectedHwid, hwidAsync);
    }
}