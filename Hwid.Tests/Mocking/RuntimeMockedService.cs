using System.Runtime.InteropServices;
using Majipro.Hwid.Services.Runtime;

namespace Majipro.Hwid.Tests.Mocking;

internal sealed class RuntimeMockedService : IRuntimeService
{
    public OSPlatform BehaveLikePlatform { get; set; }
    
    public bool IsOSPlatform(OSPlatform osPlatform)
    {
        return BehaveLikePlatform == osPlatform;
    }
}