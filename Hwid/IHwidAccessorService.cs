using System.Threading;
using System.Threading.Tasks;

namespace Majipro.Hwid;

public interface IHwidAccessorService
{
    ValueTask<string> GetHwidAsync(CancellationToken cancellationToken = default);
    
    string GetHwid(CancellationToken cancellationToken = default);
}