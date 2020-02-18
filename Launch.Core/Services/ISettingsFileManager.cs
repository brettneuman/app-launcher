using System.Threading;
using System.Threading.Tasks;

namespace Launch.Core.Services
{
    public interface ISettingsFileManager
    {
        Task<SettingsModel> ParseSettingsFile(CancellationToken cancellationToken = default);
        Task WriteSettingsFile(SettingsModel model, CancellationToken cancellationToken = default);
    }
}