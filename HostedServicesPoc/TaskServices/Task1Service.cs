using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServicesPoc.TaskServices
{
    public class Task1Service : ITask1Service
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            GlobalVariables.TraceLogId = Guid.NewGuid();
            Console.WriteLine($"Task1 executing with traceLogId = {GlobalVariables.TraceLogId}");
            Console.WriteLine($"Task1 will wait 5 seconds = {GlobalVariables.TraceLogId}");
            await Task.Delay(5000, cancellationToken);
            Console.WriteLine($"Task1 ending = {GlobalVariables.TraceLogId}");
        }
    }
}