using AutoMapper;
using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Infrastructure.Metric;
using CMS.API.Campaign.Infrastructure.Redis;
using CMS.API.Campaign.WebApi.Controllers;
using CMS.API.Campaign.WebApi.Requests;
using CMS.API.Campaign.WebApi.Util;
using CMS.API.Campaign.WebApi.Validators;
using FluentValidation;
using iHerb.CMS.Cache.Redis.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/api/HealthCheck", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (swagger.Servers.Count > 0)
                    {
                        swagger.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                        };
                    }
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "iHerb CMS Campaign Api");
            });
            app.UseResponseCompression();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    internal static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddNewtonsoftJson()
                .AddControllersAsServices()
                ;

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            var profiles = new Type[] { typeof(AutomapperProfile) };
            services.AddAutoMapper(profiles);
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
                options.DocumentFilter<EnumDescriptionDocumentFilter>();
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "iHerb CMS Campaign Api",
                    Version = "v1",
                    Description = "The CMS Campaign http api for web/mobile/app",
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{typeof(BannerController).Assembly.GetName().Name}.xml");
                options.IncludeXmlComments(xmlPath, true);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();

            services.AddSingleton<IRedisAccess, RedisAccess>();
            services.AddSingleton<IMetricClient, MetricClient>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IBannerService, BannerService>();

            services.AddTransient<IValidator<GetBannerSummariesRequest>, GetCampaignBannersRequestValidator>();

            services.AddRedisCache<CampaignSubscriptionConverter>(config =>
            {
                config.RedisConfig = new iHerb.CMS.Cache.Redis.Model.RedisConfig()
                {
                    Host = configuration["RedisConfig:ConnectionString"],
                };
                config.KeyPrefix = configuration["RedisConfig:CampaignBannerSetName"];
                config.SubscribeChannel = $"{configuration["RedisConfig:CampaignBannerSetName"]}-Channel";
            });

            return services;
        }

        public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RedisConfig>(configuration.GetSection("RedisConfig"));
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            return services;
        }
    }
}