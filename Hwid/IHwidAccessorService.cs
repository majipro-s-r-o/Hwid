using System.Threading;
using System.Threading.Tasks;

namespace Majipro.Hwid;

public interface IHwidAccessorService
{
    /// <summary>
    /// Gets HWID.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask{string}"/> containing HWID.</returns>
    ValueTask<string> GetHwidAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets HWID.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="string"/> containing HWID.</returns>
    string GetHwid(CancellationToken cancellationToken = default);
}