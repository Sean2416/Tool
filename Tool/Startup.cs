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
                ServerName = String.Format($"{Environment.MachineName}.{Guid.NewGuid()}"), //�w�]�ϥιq���W��:ProcessID�A�Ҧp�Gserver1:9853�Aserver1:4531�Aserver2:6742
                WorkerCount = 20, //������ϥμƶq�A�w�] 20 
                Queues = new[] { "default" }, //��C�W�١A�i�H�h���A�w�] default�A�p�G�A�����ȷQ�n�ΧO���W�١A�@�}�l�N�n�ŧi
                CancellationCheckInterval = TimeSpan.FromSeconds(5), //���Ȩ����ˬd�g���A�w�] 00:00:05 (5��)
                ServerTimeout = TimeSpan.FromMinutes(5),//�A�ȹO�ɡA�w�] 00:05:00 (5 ����)
                ServerCheckInterval = TimeSpan.FromMinutes(5), //�A���ˬd�g���A�w�] 00:05:00 (5 ����)
                SchedulePollingInterval = TimeSpan.FromSeconds(15), //����Ƶ{���Ȫ����߶g���A�w�] 00:00:15 (15��)�A�C 15 �����@������
                ShutdownTimeout = TimeSpan.FromSeconds(15), //�����O�ɡA�w�] 00:00:15 (15��)
                StopTimeout = TimeSpan.FromSeconds(15), //����O�ɡA�w�] 00:00:00
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
