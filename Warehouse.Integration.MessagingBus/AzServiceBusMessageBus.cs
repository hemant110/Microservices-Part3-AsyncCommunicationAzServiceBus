using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Warehouse.Integration.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Integration.MessagingBus
{
    public class AzServiceBusMessageBus : IMessageBus
    {
        private string connectionString =
        "Endpoint=sb://globoticket-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ojsO8Tkvx6p/ZCO5tkmIbAyr/0ygKkGGrbdj370BtQ8=";
        //"Endpoint=sb://globoticket-dev.servicebus.windows.net/;SharedAccessKeyName=RootManagesharedAccessKey;SharedAccesskey=ojsO8Tkvx6p/ZCO5tkmIbAyr/0ygKkGGrbdj370BtQ8=";

        public async Task PublishMessage(IntegrationBaseMessage message, string topicName)
        {
            ISenderClient topicClient = new TopicClient(connectionString, topicName);
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(message);
                var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    CorrelationId = Guid.NewGuid().ToString()
                };

                await topicClient.SendAsync(serviceBusMessage);
                Console.WriteLine($"Send message to {topicClient}");
                await topicClient.CloseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await topicClient.CloseAsync();
            }
        }
    }
}
