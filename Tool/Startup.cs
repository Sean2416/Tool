using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Configs;
using Tool.Helpers;
using Tool.Tasks;
using Tradevan_Hangfire;

namespace Tool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Config>(Configuration.GetSection("Config"));
            services.AddScoped<FileHelper>();

            var connectionString = Configuration["HangfireConfig:ConnectionString"];

            services.HangfireServices<ScheduleManagerExtension>(Configuration, config =>
            {
                config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true // Migration to Schema 7 is required
                });
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tool List API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                 url: "/swagger/v1/swagger.json",
                 name: "RESTful API v1.0.0"
                );
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.ToUseHangfireServer(new BackgroundJobServerOptions()
            {
                ServerName = String.Format($"{Environment.MachineName}.{Guid.NewGuid()}"), //預設使用電腦名稱:ProcessID，例如：server1:9853，server1:4531，server2:6742
                WorkerCount = 20, //執行緒使用數量，預設 20 
                Queues = new[] { "default" }, //佇列名稱，可以多筆，預設 default，如果你的任務想要用別的名稱，一開始就要宣告
                CancellationCheckInterval = TimeSpan.FromSeconds(5), //任務取消檢查週期，預設 00:00:05 (5秒)
                ServerTimeout = TimeSpan.FromMinutes(5),//服務逾時，預設 00:05:00 (5 分鐘)
                ServerCheckInterval = TimeSpan.FromMinutes(5), //服務檢查週期，預設 00:05:00 (5 分鐘)
                SchedulePollingInterval = TimeSpan.FromSeconds(15), //執行排程任務的輪詢週期，預設 00:00:15 (15秒)，每 15 秒執行一次任務
                ShutdownTimeout = TimeSpan.FromSeconds(15), //關閉逾時，預設 00:00:15 (15秒)
                StopTimeout = TimeSpan.FromSeconds(15), //停止逾時，預設 00:00:00
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
