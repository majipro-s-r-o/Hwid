using System.Runtime.InteropServices;

namespace Majipro.Hwid.Services.HwidProvision;

internal sealed class WindowsHwidProvisionService : IHwidProvisionService
{
    public OSPlatform Platform { get; } = OSPlatform.Windows;
    
    public string Command { get; } = "powershell";

    public string[] Args { get; } = ["-Command \"(Get-WmiObject -Class Win32_ComputerSystemProduct).UUID\""];
    
    public bool ParseHwidFromStdOut(string line, out string hwid)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            hwid = default;
            return false;
        }
        
        hwid = line.Trim();
        return true;
    }
}