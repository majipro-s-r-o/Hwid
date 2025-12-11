using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Majipro.Hwid.Services.HwidOptions;
using Majipro.Hwid.Services.Runtime;

namespace Majipro.Hwid.Services.ProcessLauncher;

internal sealed class ProcessLauncherService : IProcessLauncherService
{
    private readonly IHwidOptionsService _hwidOptionsService;

    public ProcessLauncherService(IHwidOptionsService hwidOptionsService)
    {
        _hwidOptionsService = hwidOptionsService;
    }

    public async IAsyncEnumerable<string> LaunchProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken, string command, params string[] args)
    {
        using var process = GetProcess(command, args);
        
        process.Start();

        while (process.HasExited == false)
        {
            string line = await process.StandardOutput.ReadLineAsync();
            
            while (line != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    process.Kill();

                    throw new OperationCanceledException();
                }
                
                yield return line;
                
                line = await process.StandardOutput.ReadLineAsync();
            }
        }

        WaitForExit(process);
    }
    
    public IEnumerable<string> LaunchProcess(CancellationToken cancellationToken, string command, params string[] args)
    {
        using var process = GetProcess(command, args);
        
        process.Start();

        while (process.HasExited == false)
        {
            string line = process.StandardOutput.ReadLine();
            
            while (line != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    process.Kill();

                    throw new OperationCanceledException();
                }
                
                yield return line;
                
                line = process.StandardOutput.ReadLine();
            }
        }

        WaitForExit(process);
    }

    private void WaitForExit(Process process)
    {
        if (_hwidOptionsService.ProcessTimeout == null)
        {
            process.WaitForExit();
        }
        else
        {
            int ms = Convert.ToInt32(Math.Abs(_hwidOptionsService.ProcessTimeout.Value.TotalMilliseconds));
            process.WaitForExit(ms);
        }
    }
    
    private Process GetProcess(string command, params string[] args)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = string.Join(' ', args),
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return new Process
        {
            StartInfo = processStartInfo
        };
    }
}