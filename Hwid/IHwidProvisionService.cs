using System.Runtime.InteropServices;

namespace Majipro.Hwid;

public interface IHwidProvisionService
{
    /// <summary>
    /// Defines the platform for this provision service.
    /// </summary>
    OSPlatform Platform { get; }
    
    /// <summary>
    /// Command to execute.
    /// </summary>
    string Command { get; }

    /// <summary>
    /// Command arguments.
    /// </summary>
    string[] Args { get; }
    
    /// <summary>
    /// Parses HWID from standard output.
    /// </summary>
    /// <param name="line"><see cref="string"/> contains the current line of standard output.</param>
    /// <param name="hwid"><see cref="string"/> containing the parsed HWID.</param>
    /// <returns><see cref="bool"/> indicating whether HWID was successfully parsed.</returns>
    bool ParseHwidFromStdOut(string line, out string hwid);
}