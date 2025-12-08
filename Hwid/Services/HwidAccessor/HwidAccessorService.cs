using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Majipro.Hwid.Services.ProcessLauncher;

namespace Majipro.Hwid.Services.HwidAccessor;

internal sealed class HwidAccessorService : IHwidAccessorService
{
    private readonly IEnumerable<IHwidProvisionService> _hwidProvisionServices;
    private readonly IProcessLauncherService _processLauncherService;

    public HwidAccessorService(IEnumerable<IHwidProvisionService> hwidProvisionServices, IProcessLauncherService processLauncherService)
    {
        _hwidProvisionServices = hwidProvisionServices;
        _processLauncherService = processLauncherService;
    }

    public async ValueTask<string> GetHwidAsync(CancellationToken cancellationToken = default)
    {
        var provisioningService = ResolveProvisioningServiceByPlatform();

        var rows = _processLauncherService.LaunchProcessAsync(cancellationToken, provisioningService.Command, provisioningService.Args);

        await foreach (var row in rows)
        {
            if (provisioningService.ParseHwidFromStdOut(row, out string hwid))
            {
                return GetNormalizedHwid(hwid);
            }
        }
        
        throw new InvalidOperationException("Unable to get hwid");
    }

    public string GetHwid(CancellationToken cancellationToken = default)
    {
        var provisioningService = ResolveProvisioningServiceByPlatform();

        var rows = _processLauncherService.LaunchProcess(cancellationToken, provisioningService.Command, provisioningService.Args);

        foreach (var row in rows)
        {
            if (provisioningService.ParseHwidFromStdOut(row, out string hwid))
            {
                return GetNormalizedHwid(hwid);
            }
        }
        
        throw new InvalidOperationException("Unable to get hwid");
    }

    private static string GetNormalizedHwid(string hwid)
    {
        using var sha1 = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(hwid);
        var hash = sha1.ComputeHash(bytes);
        var sb = new StringBuilder(hash.Length * 2);

        foreach (var b in hash)
        {
            sb.Append(b.ToString("X2"));
        }
        
        return sb.ToString();
    }

    private IHwidProvisionService ResolveProvisioningServiceByPlatform()
    {
        var hwidProvisioningService = _hwidProvisionServices
            .LastOrDefault(s => RuntimeInformation.IsOSPlatform(s.Platform));

        if (hwidProvisioningService == null)
        {
            throw new PlatformNotSupportedException();
        }

        return hwidProvisioningService;
    }
}