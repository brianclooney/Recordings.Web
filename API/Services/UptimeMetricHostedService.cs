using System.Diagnostics;
using Prometheus;

namespace Recordings.API.Services
{
    public class UptimeMetricHostedService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly Stopwatch _stopwatch;
        public static readonly Gauge ApplicationUptime = Metrics
            .CreateGauge("application_uptime_seconds", "The total time the application has been running.");

        public UptimeMetricHostedService()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Update the gauge every 5 seconds
            _timer = new Timer(UpdateUptimeMetric, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private void UpdateUptimeMetric(object? state)
        {
            ApplicationUptime.Set(_stopwatch.Elapsed.TotalSeconds);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
