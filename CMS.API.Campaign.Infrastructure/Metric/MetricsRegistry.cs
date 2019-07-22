using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;

namespace CMS.API.Campaign.Infrastructure.Metric
{
    public class MetricsRegistry
    {
        public static TimerOptions BuildTimerOptions(string name)
        {
            return new TimerOptions
            {
                Name = name ?? "Request Timer",
                MeasurementUnit = Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };
        }
        public static CounterOptions BuildCounterOptions(string name)
        {
            return new CounterOptions
            {
                Name = name ?? "Request Counter",
                MeasurementUnit = Unit.Calls
            };
        }
    }
}
