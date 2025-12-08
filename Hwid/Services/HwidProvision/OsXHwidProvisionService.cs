using System;
using System.Runtime.InteropServices;

namespace Majipro.Hwid.Services.HwidProvision;

internal sealed class OsXHwidProvisionService : IHwidProvisionService
{
    public OSPlatform Platform { get; } = OSPlatform.OSX;
    
    public string Command { get; } = "ioreg";
 
    public string[] Args { get; } = ["-rd1", "-c", "IOPlatformExpertDevice"];

    private const string Key = "IOPlatformSerialNumber";
    
    public bool ParseHwidFromStdOut(string line, out string hwid)
    {
        var span = line
            .Trim()
            .AsSpan();

        bool containsKey = span.StartsWith('"' + Key);

        if (containsKey == false)
        {
            hwid = default;
            return false;
        }

        int equalIndex = span.IndexOf('=');
        int expectedLength = span.Length - equalIndex - 4;

        if (equalIndex <= 0 || expectedLength <= 0)
        {
            hwid = default;
            return false;
        }
        
        hwid = span
            .Slice(equalIndex + 3, expectedLength)
            .Trim()
            .ToString();
        return true;
    }
}