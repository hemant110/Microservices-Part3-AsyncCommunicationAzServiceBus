using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderReceive.DBContexts;
using OrderReceive.Repositories;
using OrderReceive.Services;
using Warehouse.Integration.MessagingBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderReceive.Worker;

namespace OrderReceive
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
            services.AddHostedService<ServiceBusListenerOH>();

            services.AddHostedService<ServiceBusListenerOL>();

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddHostedService<ServiceBusListener>();

            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddScoped<IOrderLinesRepository, OrderLineRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderReceiveDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            services.AddSingleton(new OrderHeaderRepository(optionsBuilder.Options));
            services.AddSingleton(new OrderLineRepository(optionsBuilder.Options));

            services.AddHttpClient<IQualityCheckService, QualityCheckService>(c =>
                c.BaseAddress = new Uri(Configuration["ApiConfigs:QualityCheck:Uri"]));

            services.AddSingleton<IMessageBus, AzServiceBusMessageBus>();

            services.AddDbContext<OrderReceiveDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Receive API", Version = "v1" });
                c.ResolveConflictingActions(apiDesc => apiDesc.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Order Receive API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
