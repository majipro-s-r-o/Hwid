using System.Runtime.InteropServices;

namespace Majipro.Hwid.Services.HwidProvision;

internal sealed class LinuxHwidProvisionService : IHwidProvisionService
{
    public OSPlatform Platform { get; } = OSPlatform.Linux;
    
    public string Command { get; } = "/usr/bin/sudo";

    public string[] Args { get; } = ["/usr/bin/cat", "/sys/class/dmi/id/product_uuid"];
    
    public bool ParseHwidFromStdOut(string line, out string hwid)
    {
        string trimmedLine = line?.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.EndsWith("Permission denied")) // Should not be localized
        {
            hwid = default;
            return false;
        }
        
        hwid = trimmedLine;
        return true;
    }
}