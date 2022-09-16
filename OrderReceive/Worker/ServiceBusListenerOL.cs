using AutoMapper;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrderReceive.Entities;
using OrderReceive.Messages;
using OrderReceive.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderReceive.Worker
{
    public class ServiceBusListenerOL : IHostedService
    {
        private readonly IConfiguration configuration;
        private ISubscriptionClient subscriptionClient;
        private readonly OrderLineRepository orderLinesRepository;
        private readonly IMapper mapper;

        private readonly string orderLineSub = "orderLineSub";
        private readonly string sbConnectionString;
        private readonly string orderLineUpdateTopic;

        public ServiceBusListenerOL(IConfiguration configuration, OrderLineRepository orderLinesRepository, IMapper mapper)
        {
            this.configuration = configuration;
            this.orderLinesRepository = orderLinesRepository;
            this.mapper = mapper;

            sbConnectionString = this.configuration.GetValue<string>("ServiceBusConnectionString");
            orderLineUpdateTopic = this.configuration.GetValue<string>("OrderLineUpdateTopic");

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                subscriptionClient = new SubscriptionClient(sbConnectionString, orderLineUpdateTopic, orderLineSub);
                subscriptionClient.PrefetchCount = 0;

                var messageHandler = new MessageHandlerOptions(e =>
                 {
                     ProcessError(e.Exception);
                     return Task.CompletedTask;

                 })
                {
                    MaxAutoRenewDuration = TimeSpan.FromMinutes(1),
                    AutoComplete = false
                    ,
                    MaxConcurrentCalls = 1
                };

                subscriptionClient.RegisterMessageHandler(UpdateLineAsync, messageHandler);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in StartAsync of Order Receive.");
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }

        private async Task UpdateLineAsync(Message msg, CancellationToken arg2)
        {
            try
            {
                var msgBody = Encoding.UTF8.GetString(msg.Body);
                OrderLineUpdateMessage orderLineUpdateMessage = JsonConvert.DeserializeObject<OrderLineUpdateMessage>(msgBody);

                OrderLines orderLine = await orderLinesRepository.GetOrderLinesById(orderLineUpdateMessage.Order_Tag);

                orderLine.QtyAllocated = orderLineUpdateMessage.Qty_Passed;

                await orderLinesRepository.AddOrUpdateOrderLine(orderLineUpdateMessage.Order_Code, orderLine);
                await orderLinesRepository.SaveChanges();

                await subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);

                Console.WriteLine($"Order Line service received and processed message");

                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in UpdateLineAsync of Order Receive.");
                Console.WriteLine(ex.Message);
            }
        }

        private void ProcessError(Exception exception)
        {
            Console.WriteLine("Error occured during order header subscription");
            Console.WriteLine(exception.Message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await subscriptionClient.CloseAsync();
        }
    }
}
