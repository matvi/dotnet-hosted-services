using System.Threading;
using System.Threading.Tasks;

namespace HostedServicesPoc.TaskServices
{
    public interface ITaskService
    {
        public Task StartAsync(CancellationToken cancellationToken);
    }
}