using System;

namespace Majipro.Hwid;

public interface IHwidOptions
{
    /// <summary>
    /// Timeout for process execution. When null, there is no timeout.
    /// </summary>
    TimeSpan? ProcessTimeout { get; set; }
}