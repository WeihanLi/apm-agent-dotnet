using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CMS.API.Campaign.WebApi
{
    /// <summary>
    /// main class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// metrics
        /// </summary>
        public static IMetricsRoot Metrics { get; set; }
        /// <summary>
        /// main point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// start up
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();

            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureMetrics(Metrics)
                .UseMetricsWebTracking(options =>
                {
                    options.IgnoredRoutesRegexPatterns = new[] { "swagger/index.html", "api/hc" };
                    options.OAuth2TrackingEnabled = false;
                    options.ApdexTrackingEnabled = true;
                    options.ApdexTSeconds = 0.5;
                    options.IgnoredHttpStatusCodes = new[] { 404 };
                })
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter =
                            Metrics.OutputMetricsFormatters.GetType(typeof(MetricsPrometheusTextOutputFormatter));
                        endpointsOptions.MetricsEndpointOutputFormatter =
                            Metrics.OutputMetricsFormatters.GetType(typeof(MetricsPrometheusProtobufOutputFormatter));
                    };
                })
                .UseStartup<Startup>();
        }
    }
}