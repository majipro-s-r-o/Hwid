using System.Runtime.InteropServices;

namespace Majipro.Hwid.Tests.Mocking;

internal sealed class HwidProvisionMockedService : IHwidProvisionService
{
    public OSPlatform BehaveLikePlatform { get; set; }
    
    public OSPlatform Platform => BehaveLikePlatform;
    public string Command { get; }
    public string[] Args { get; }
    
    public bool ParseHwidFromStdOut(string line, out string hwid)
    {
        hwid = line;
        return true;
    }
}