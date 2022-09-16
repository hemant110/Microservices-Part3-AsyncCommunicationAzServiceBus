using Microsoft.AspNetCore.Builder;
using QCService.MessageConsumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace QCService.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IServiceBusConsumer>();

            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(onStarted);
            hostApplicationLifetime.ApplicationStopped.Register(onStopped);

            return app;

        }

        private static void onStopped()
        {
            ServiceBusConsumer.stop();
        }

        private static void onStarted()
        {
            ServiceBusConsumer.start();
        }
    }
}
