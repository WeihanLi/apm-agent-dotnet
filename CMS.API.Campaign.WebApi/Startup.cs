using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Domain.Repositories;
using CMS.API.Campaign.Infrastructure.Metric;
using CMS.API.Campaign.Infrastructure.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
            //configure
            services.Configure<RedisConfig>(Configuration.GetSection("RedisConfig"));
            services.Configure<SlotImageConfig>(Configuration.GetSection("SlotImageConfig"));
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
            //application
            services.AddSingleton<ISlotService, SlotService>();

            //repository
            services.AddSingleton<ISlotRepository, SlotRepository>();

            //infrastructure
            services.AddSingleton<IRedisAccess, RedisAccess>();
            //middleware
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "iHerb CMS Campaign Api", Version = "v1" });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Xml/CMS.API.Campaign.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMvc();
            services.AddResponseCaching();

            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            //metric
            services.AddSingleton<IMetricClient, MetricClient>();
            services.AddMetrics();
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

            app.UseHealthChecks("/api/home/index", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "iHerb CMS Campaign Api"); });
            app.UseResponseCompression();
            app.UseMvc();
            app.UseResponseCaching();
        }
    }
}
