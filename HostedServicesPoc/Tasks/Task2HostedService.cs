using System;
using System.Threading;
using System.Threading.Tasks;
using HostedServicesPoc.TaskServices;
using HostedServicesPoc.TaskSettings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HostedServicesPoc.Tasks
{
    public class Task2HostedService : CronJobServiceBase
    {
        private readonly IServiceProvider _serviceProvider;

        public Task2HostedService(
            IOptions<Task2HostedServiceSettings> hostedServiceSettings,
            ILogger<CronJobServiceBase> log,
            IServiceProvider serviceProvider) : base(hostedServiceSettings, log)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var task2Service = scope.ServiceProvider.GetRequiredService<ITask2Service>();
            await task2Service.StartAsync(cancellationToken);
        }
    }
}