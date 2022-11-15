using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServicesPoc.TaskServices
{
    public class Task1Service : ITask1Service
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Task1 executing");
            return Task.CompletedTask;
        }
    }
}