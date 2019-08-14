using App.Metrics.Timer;

namespace CMS.API.Campaign.Infrastructure.Metric
{
    public interface IMetricClient
    {
        TimerContext Timer(string name);
        void Counter(string name);
        void Histogram(string name, long value);
        void Meter(string name);
    }
}
