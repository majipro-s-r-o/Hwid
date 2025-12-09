using System.Collections.Generic;
using System.Threading;

namespace Majipro.Hwid.Services.ProcessLauncher;

internal interface IProcessLauncherService
{
    IAsyncEnumerable<string> LaunchProcessAsync(CancellationToken cancellationToken, string command, params string[] args);

    IEnumerable<string> LaunchProcess(CancellationToken cancellationToken, string command, params string[] args);
}