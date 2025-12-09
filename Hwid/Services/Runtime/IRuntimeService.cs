using System.Runtime.InteropServices;

namespace Majipro.Hwid.Services.Runtime;

internal interface IRuntimeService
{
    bool IsOSPlatform(OSPlatform osPlatform);
}