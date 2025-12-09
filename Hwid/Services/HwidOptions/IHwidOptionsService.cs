using System;

namespace Majipro.Hwid.Services.HwidOptions;

internal interface IHwidOptionsService
{
    TimeSpan? ProcessTimeout { get; }
}