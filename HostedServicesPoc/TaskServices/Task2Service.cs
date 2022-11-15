using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServicesPoc.TaskServices
{
    public class Task2Service : ITask2Service
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Task2 executing");
            return Task.CompletedTask;
        }
    }
}