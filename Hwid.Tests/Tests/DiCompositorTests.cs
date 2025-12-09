using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Majipro.Hwid.Services.HwidOptions;
using Majipro.Hwid.Services.ProcessLauncher;
using Majipro.Hwid.Services.Runtime;
using Majipro.Hwid.Tests.Mocking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Majipro.Hwid.Tests.Tests;

[TestClass]
public class DiCompositorTests
{
    [TestMethod]
    public async Task When_Custom_Provisioning_Service_Is_Registered_Then_Used_That_Service()
    {
        // Arrange
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddHwid(w => w.AddProvisionService<HwidProvisionMockedService>());

                services.AddSingleton<IProcessLauncherService, ProcessLauncherMockedService>();
                services.AddSingleton<IRuntimeService, RuntimeMockedService>();
            })
            .Build();
        
        var hwidProvisionService = host.Services.GetRequiredService<IHwidProvisionService>() as HwidProvisionMockedService;
        var runtimeService = host.Services.GetRequiredService<IRuntimeService>() as RuntimeMockedService;
        var processLauncherService = host.Services.GetRequiredService<IProcessLauncherService>() as ProcessLauncherMockedService;
        var hwidAccessorService = host.Services.GetRequiredService<IHwidAccessorService>();
        
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
    
    [TestMethod]
    public void When_Custom_Non_Default_Options_Is_Provided_Then_It_Should_Be_Used()
    {
        // Arrange
        var timeout = TimeSpan.FromHours(12);
        
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddHwid(o => o.ProcessTimeout = timeout);
            })
            .Build();
        
        // Act & Assert
        var hwidOptionsService = host.Services.GetRequiredService<IHwidOptionsService>();
        
        Assert.AreEqual(timeout, hwidOptionsService.ProcessTimeout);
    }
}