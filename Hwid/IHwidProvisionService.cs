using System.Runtime.InteropServices;

namespace Majipro.Hwid;

public interface IHwidProvisionService
{
    OSPlatform Platform { get; }
    
    string Command { get; }

    string[] Args { get; }
    
    bool ParseHwidFromStdOut(string line, out string hwid);
}