# HWID
Cross-platform library for obtaining a unique hardware identifier.

## What it is
This library is used to get unique hardware identifier. Such an identifier should be immutable until some significant hardware changes will not occur. It works on Windows, OSX and Linux (however, here you will need to have permissions). It runs a process behind the scenes that tries to fetch unique hardware identifier, it parses the result and hashes the actual identifier. If you are not satisfied with used strategies to fetch hardware identifier, you can write your own using this library as well. The whole implementation is built around Dependency Injection, so if you are not using it, you will most probably not appreciate this library.

### How to register it
The basic registration is as follows:
```csharp
builder.Services.AddHwid();
```
You can enhance this registration by providing your own implementation of `IHardwareIdentifierFetcher` interface, which allows you to apply custom strategy how hardware identifier is fetched:
```csharp
builder.Services.AddHwid(w => w
    .AddProvisionService<CustomHwidWindowsProvisionService>()
    .AddProvisionService<CustomHwidLinuxProvisionService>());
```
You can also define custom settings, such as process timeout:
```csharp
services.AddHwid(o => o.ProcessTimeout = TimeSpan.FromSeconds(10));
```

### How to get hardware identifier
You simply inject `IHwidAccessorService` service and call:
```csharp
string hwid = _hwidAccessorService.GetHwid();
```
Optionally, you can use async overload:
```csharp
string hwid = await _hwidAccessorService.GetHwidAsync();
```
Service is registered as singleton and caching the result, so you will have to call the `IHardwareIdentifierFetcher` just once, in other calls the result from the first call will be used.

### How to create custom `IHwidAccessorService`
In certain cases you maybe don't like how hardware identifier is fetched. For such cases you can create your own implementation:
```csharp
internal sealed class CustomWindowsHwidProvisionService : IHwidProvisionService
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
```
Do not forget to register it, see "How to register it" section above.

## Current strategies to fetch hardware identifier
- Windows: `Get-WmiObject -Class Win32_ComputerSystemProduct).UUID`
- Linux: `cat /sys/class/dmi/id/product_uuid`
- OSX: `ioreg -rd1 -c IOPlatformExpertDevice`
