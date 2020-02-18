using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Launch.Core.Domain;

namespace Launch.Core.Services
{
    public interface ISettingsManager
    {
        Task<IEnumerable<Application>> GetApplications(CancellationToken cancellationToken = default);
        Task<Application> GetApplication(string app, CancellationToken cancellationToken = default);
        Task<IEnumerable<Credential>> GetCredentials(CancellationToken cancellationToken = default);
        Task<Credential> GetCredential(string cred, CancellationToken cancellationToken = default);
        Task SaveApplication(Application application, CancellationToken cancellationToken = default);
        Task SaveCredential(Credential credential, CancellationToken cancellationToken = default);
    }
}