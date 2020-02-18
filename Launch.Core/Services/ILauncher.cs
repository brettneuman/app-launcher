using System.Diagnostics;
using System.Threading.Tasks;

namespace Launch.Core.Services
{
    public interface ILauncher
    {
        public Task<Process> Launch(LaunchOptions options);
    }
}
