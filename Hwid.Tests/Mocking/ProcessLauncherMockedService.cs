using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Majipro.Hwid.Services.ProcessLauncher;

namespace Majipro.Hwid.Tests.Mocking;

internal sealed class ProcessLauncherMockedService : IProcessLauncherService
{
    public string MockedOutput { get; set; }
    
    public IAsyncEnumerable<string> LaunchProcessAsync(CancellationToken cancellationToken, string command, params string[] args)
    {
        return new List<string>
        {
            MockedOutput
        }.ToAsyncEnumerable();
    }

    public IEnumerable<string> LaunchProcess(CancellationToken cancellationToken, string command, params string[] args)
    {
        return new List<string>
        {
            MockedOutput
        };
    }
}