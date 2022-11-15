using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HostedServicesPoc.Tasks
{
    public abstract class CronJobServiceBase : IHostedService, IDisposable
    {
        private readonly ILogger _log;
        private readonly HostedServiceTaskSettingsBase _hostedServiceTaskSettingsBase;
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobServiceBase(IOptions<HostedServiceTaskSettingsBase> hostedServiceSettings, ILogger<CronJobServiceBase> log)
        {
            _log = log;
            _hostedServiceTaskSettingsBase = hostedServiceSettings?.Value;
            _expression = CronExpression.Parse(_hostedServiceTaskSettingsBase.CronExpressionTimer, CronFormat.Standard);
            _timeZoneInfo = TimeZoneInfo.Local;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation($"{GetType()} is Starting");

        #if !DEBUG
            if (_hostedServiceTaskSettingsBase.Active)
            {
                await ExecuteTaskAsync(cancellationToken);
            }
        #else
            if (_hostedServiceTaskSettingsBase.Active)
            {
                await ScheduleJob(cancellationToken);
            }

        #endif
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation($"{GetType()} is Stopping");
            return Task.CompletedTask;
        }

        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ExecuteTaskAsync(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        protected virtual async Task ExecuteTaskAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            try
            {
                if (dispose)
                {
                    _timer?.Dispose();
                }
            }
            finally
            {

            }
        }
    }
}