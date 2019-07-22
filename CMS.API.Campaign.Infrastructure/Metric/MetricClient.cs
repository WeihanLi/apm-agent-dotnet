using App.Metrics;
using App.Metrics.Timer;

namespace CMS.API.Campaign.Infrastructure.Metric
{
    public class MetricClient : IMetricClient
    {
        private readonly IMetrics _metric;

        public MetricClient(IMetrics metric)
        {
            _metric = metric;
        }

        public TimerContext Timer(string name)
        {
            var option = MetricsRegistry.BuildTimerOptions(name);
            var context = _metric.Measure.Timer.Time(option);
            return context;
        }

        public void Counter(string name)
        {
            var option = MetricsRegistry.BuildCounterOptions(name);
            _metric.Measure.Counter.Increment(option);
        }
    }
}
