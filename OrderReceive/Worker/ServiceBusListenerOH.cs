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
    public class ServiceBusListenerOH : IHostedService
    {
        private readonly IConfiguration configuration;
        private ISubscriptionClient subscriptionClient;
        private readonly OrderHeaderRepository orderHeaderRepository;
        private readonly IMapper mapper;

        private readonly string orderHeaderSub = "orderHeaderSub";
        private readonly string sbConnectionString;
        private readonly string orderHeaderUpdateTopic;

        public ServiceBusListenerOH(IConfiguration configuration, OrderHeaderRepository orderHeaderRepository, IMapper mapper)
        {
            this.configuration = configuration;
            this.orderHeaderRepository = orderHeaderRepository;
            this.mapper = mapper;

            sbConnectionString = this.configuration.GetValue<string>("ServiceBusConnectionString");
            orderHeaderUpdateTopic = this.configuration.GetValue<string>("OrderHeaderUpdateTopic");

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                subscriptionClient = new SubscriptionClient(sbConnectionString, orderHeaderUpdateTopic, orderHeaderSub);

                var messageHandler = new MessageHandlerOptions(e =>
                 {
                     ProcessError(e.Exception);
                     return Task.CompletedTask;

                 })
                {
                    AutoComplete = false
                    ,
                    MaxConcurrentCalls = 4
                };

                subscriptionClient.RegisterMessageHandler(UpdateOrderAsync, messageHandler);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in StartAsync of Order Receive.");
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }

        }

        private async Task UpdateOrderAsync(Message msg, CancellationToken arg2)
        {
            try
            {
                var msgBody = Encoding.UTF8.GetString(msg.Body);
                OrderHeaderUpdateMessage orderHeaderUpdateMessage = JsonConvert.DeserializeObject<OrderHeaderUpdateMessage>(msgBody);

                OrderHeader orderHeader = await orderHeaderRepository.GetOrderByID(orderHeaderUpdateMessage.Order_Code);

                orderHeader.Order_Status = orderHeaderUpdateMessage.Order_Status;

                orderHeaderRepository.AddOrder(orderHeader);
                await orderHeaderRepository.SaveChanges();

                await subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);

                Console.WriteLine($"Order Header received and processed message");

                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in UpdateOrderAsync of Inventory.");
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
