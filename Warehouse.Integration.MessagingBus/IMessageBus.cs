using Warehouse.Integration.Messages;
using System;
using System.Threading.Tasks;

namespace Warehouse.Integration.MessagingBus
{
    public interface IMessageBus
    {
        Task PublishMessage(IntegrationBaseMessage message, string topicName);
    }
}
