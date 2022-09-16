using AutoMapper;
using InventoryService.Entities;
using InventoryService.Messages;
using InventoryService.Repositories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Integration.MessagingBus;

namespace InventoryService.Worker
{
    public class ServiceBusListener : IHostedService
    {
        private readonly IConfiguration configuration;
        private ISubscriptionClient subscriptionClient;
        private readonly IMessageBus messageBus;
        private readonly IMapper mapper;
        private readonly InventoryRepository inventoryRepository;
        private readonly string inventoryInsertTopic;
        private readonly string orderupdateTopic;
        private readonly string connectionString;
        private readonly string invSubscription = "inventoryUpdateSub";

        public ServiceBusListener(IConfiguration configuration, IMessageBus messageBus, InventoryRepository inventoryRepository, IMapper mapper)
        {
            this.messageBus = messageBus;
            this.configuration = configuration;
            this.inventoryRepository = inventoryRepository;
            this.mapper = mapper;

            inventoryInsertTopic = this.configuration.GetValue<string>("InventoryUpdateMessageTopic");
            orderupdateTopic = this.configuration.GetValue<string>("OrderLineUpdateTopic");
            connectionString = this.configuration.GetValue<string>("ServiceBusConnectionString");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                subscriptionClient = new SubscriptionClient(connectionString, inventoryInsertTopic, invSubscription);

                var messageHandler = new MessageHandlerOptions(e =>
                {
                    ProcessError(e.Exception);
                    return Task.CompletedTask;
                })
                {
                    AutoComplete = false,
                    MaxConcurrentCalls = 4
                };

                subscriptionClient.RegisterMessageHandler(CreateInventoryAsync, messageHandler);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in StartAsync of Inventory.");
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }

        private async Task CreateInventoryAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                var msgBody = Encoding.UTF8.GetString(message.Body);

                InventoryInsertMessage inventoryInsertMessage = JsonConvert.DeserializeObject<InventoryInsertMessage>(msgBody);

                await inventoryRepository.AddUpdateInventory(mapper.Map<Inventory>(inventoryInsertMessage));

                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

                Console.WriteLine($"Inventory service received and processed message");

                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured in CreateInventoryAsync.");
                Console.WriteLine(ex.Message);
            }
        }

        private void ProcessError(Exception exception)
        {
            Console.WriteLine("Error inside ServiceBusListener");
            Console.WriteLine(exception.Message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.subscriptionClient.CloseAsync();
        }
    }
}
