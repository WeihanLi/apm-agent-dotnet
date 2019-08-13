using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Repositories;
using CMS.API.Campaign.Infrastructure.Metric;
using CMS.API.Campaign.Infrastructure.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.IO.Compression;

namespace CMS.API.Campaign.WebApi
{
    /// <summary>
    /// start up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// initialize
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc(Configuration)
                .AddHealthChecks(Configuration)
                .AddCustomSwagger(Configuration)
                .AddSettingsConfiguration(Configuration)
                .AddCustomConfiguration(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/api/HealthCheck", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "iHerb CMS Campaign Api"); });
            app.UseResponseCompression();
            
            app.UseMvc();
        }
    }

    internal static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();
           
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMemoryCache();
            services.AddMetrics();

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "iHerb CMS Campaign Api",
                    Version = "v1",
                    Description = "The CMS Campaign http api for web/mobile/app",
                    TermsOfService = "Terms Of Service"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Xml/CMS.API.Campaign.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();
            services.AddSingleton<ISlotService, SlotService>();
            services.AddSingleton<ISlotRepository, SlotRepository>();
            services.AddSingleton<IRedisAccess, RedisAccess>();
            services.AddSingleton<IMetricClient, MetricClient>();
            services.AddSingleton<ICacheRepository, CacheRepository>();

            return services;
        }

        public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RedisConfig>(configuration.GetSection("RedisConfig"));
            services.Configure<SlotImageConfig>(configuration.GetSection("SlotImageConfig"));
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            return services;
        }
    }
}
