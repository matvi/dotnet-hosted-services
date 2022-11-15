using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServicesPoc.TaskServices
{
    public class Task2Service : ITask2Service
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Task2 executing");
            GlobalVariables.TraceLogId = Guid.NewGuid();
            Console.WriteLine($"Task2 executing with traceLogId = {GlobalVariables.TraceLogId}");
            Console.WriteLine($"Task2 ending = {GlobalVariables.TraceLogId}");
        }
    }
}