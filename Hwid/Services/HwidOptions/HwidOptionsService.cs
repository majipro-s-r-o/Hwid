using System;

namespace Majipro.Hwid.Services.HwidOptions;

internal sealed class HwidOptionsService : IHwidOptions, IHwidOptionsService
{
    public TimeSpan? ProcessTimeout { get; set; } = TimeSpan.FromSeconds(30);
}