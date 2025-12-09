using System.Runtime.InteropServices;

namespace Majipro.Hwid.Services.Runtime;

internal sealed class RuntimeService : IRuntimeService
{
    public bool IsOSPlatform(OSPlatform osPlatform)
    {
        return RuntimeInformation.IsOSPlatform(osPlatform);
    }
}