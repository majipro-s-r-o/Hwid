using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Majipro.Hwid.Services.ProcessLauncher;
using Majipro.Hwid.Services.Runtime;

namespace Majipro.Hwid.Services.HwidAccessor;

internal sealed class HwidAccessorService : IHwidAccessorService
{
    private readonly IEnumerable<IHwidProvisionService> _hwidProvisionServices;
    private readonly IProcessLauncherService _processLauncherService;
    private readonly IRuntimeService _runtimeService;

    private readonly object _lock = new();
    
    private string _hwid = null;

    public HwidAccessorService(
        IEnumerable<IHwidProvisionService> hwidProvisionServices,
        IProcessLauncherService processLauncherService,
        IRuntimeService runtimeService)
    {
        _hwidProvisionServices = hwidProvisionServices;
        _processLauncherService = processLauncherService;
        _runtimeService = runtimeService;
    }

    public async ValueTask<string> GetHwidAsync(CancellationToken cancellationToken = default)
    {
        if (_hwid != null)
        {
            return _hwid;
        }
        
        var provisioningService = ResolveProvisioningServiceByPlatform();

        var rows = _processLauncherService.LaunchProcessAsync(cancellationToken, provisioningService.Command, provisioningService.Args);

        await foreach (var row in rows)
        {
            if (provisioningService.ParseHwidFromStdOut(row, out string hwid))
            {
                return SetHwid(hwid);
            }
        }
        
        throw new InvalidOperationException("Unable to get hwid");
    }

    public string GetHwid(CancellationToken cancellationToken = default)
    {
        if (_hwid != null)
        {
            return _hwid;
        }
        
        var provisioningService = ResolveProvisioningServiceByPlatform();

        var rows = _processLauncherService.LaunchProcess(cancellationToken, provisioningService.Command, provisioningService.Args);

        foreach (var row in rows)
        {
            if (provisioningService.ParseHwidFromStdOut(row, out string hwid))
            {
                return SetHwid(hwid);
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

    private string SetHwid(string hwid)
    {
        string normalizedHwid = GetNormalizedHwid(hwid);

        lock (_lock)
        {
            _hwid = normalizedHwid;
        }

        return _hwid;
    }

    private IHwidProvisionService ResolveProvisioningServiceByPlatform()
    {
        var hwidProvisioningService = _hwidProvisionServices
            .LastOrDefault(s => _runtimeService.IsOSPlatform(s.Platform));

        if (hwidProvisioningService == null)
        {
            throw new PlatformNotSupportedException();
        }

        return hwidProvisioningService;
    }
}