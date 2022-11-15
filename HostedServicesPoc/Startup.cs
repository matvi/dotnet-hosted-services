using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostedServicesPoc.Tasks;
using HostedServicesPoc.TaskServices;
using HostedServicesPoc.TaskSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace HostedServicesPoc
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HostedServicesPoc", Version = "v1" });
            });
            
            //register settings
            services.Configure<Task1HostedServiceSettings>(Configuration?.GetSection("Task1HostedServiceSettings"));
            services.Configure<Task2HostedServiceSettings>(Configuration?.GetSection("Task2HostedServiceSettings"));
            
            //register services
            services.AddScoped<ITask1Service, Task1Service>();
            services.AddScoped<ITask2Service, Task2Service>();
            
            //register hosted services
            services.AddHostedService<Task1HostedService>();
            services.AddHostedService<Task2HostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HostedServicesPoc v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}