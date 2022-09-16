using AutoMapper;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QCService.Entities;
using QCService.Messages;
using QCService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Integration.MessagingBus;

namespace QCService.MessageConsumer
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly string subscription = "qcServiceSub";
        private readonly string qcInsertMessageTopic;
        private readonly string orderHeaderUpdateTopic;


        private readonly IReceiverClient qcInsertMessageReceiveClient;

        private readonly IConfiguration configuration;

        private readonly QualityCheckRepository qualityCheckRepository;
        private readonly IMessageBus messageBus;

        private readonly IMapper mapper;


        public ServiceBusConsumer(IConfiguration configuration, IMessageBus messageBus, QualityCheckRepository qualityCheckRepository
        , IMapper mapper)
        {
            this.configuration = configuration;
            this.messageBus = messageBus;
            this.qualityCheckRepository = qualityCheckRepository;
            this.mapper = mapper;

            var serviceBusConnectionString = this.configuration.GetValue<string>("ServiceBusConnectionString");
            qcInsertMessageTopic = this.configuration.GetValue<string>("QCInsertMessageTopic");
            orderHeaderUpdateTopic = this.configuration.GetValue<string>("OrderHeaderUpdateTopic");

            this.qcInsertMessageReceiveClient = new SubscriptionClient(serviceBusConnectionString, qcInsertMessageTopic, subscription);
        }
        public void start()
        {
            try
            {
                var messageBusHandlerOptions = new MessageHandlerOptions(OnServiceBusException) { MaxConcurrentCalls = 4,
                    AutoComplete=false };
                //qcInsertMessageReceiveClient.RegisterPlugin : ToDo
                qcInsertMessageReceiveClient.RegisterMessageHandler(OnQCMessageReceive, messageBusHandlerOptions);


            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error Occured in Start method of QC Consumer");
                Console.WriteLine(ex.Message);
            }
        }

        private async Task OnQCMessageReceive(Message msg, CancellationToken arg2)
        {
            try
            {
                var messageBody = Encoding.UTF8.GetString(msg.Body); // json will be retrived from message bus

                QCInsertMessage qCInsertMessage = JsonConvert.DeserializeObject<QCInsertMessage>(messageBody);

                Guid qcId = Guid.NewGuid();

                foreach (QCLineMessage qCLineMessage in qCInsertMessage.QualityCheckList)
                {
                    QualityCheck qualityCheck = mapper.Map<QualityCheck>(qCLineMessage);

                    qualityCheck.Company_Name = qualityCheck.Company_Code;
                    qualityCheck.Customer_Name = qualityCheck.Customer_Code;
                    qualityCheck.Warehouse_Name = qualityCheck.Warehouse_Code;

                    QualityCheck qc = await qualityCheckRepository.GetQualityCheckTasksByOrderAndTag(qualityCheck.QC_List, qualityCheck.QC_Tag);
                    if(qc == null)
                    {
                        await qualityCheckRepository.AddQualityCheck(qualityCheck);
                    }
                }

                OrderHeaderUpdateMessage orderHeaderUpdateMessage = new OrderHeaderUpdateMessage()
                {
                    Order_Code = qCInsertMessage.QualityCheckList.FirstOrDefault().QC_List,
                    Order_Status = "PendinWithQC"
                };

                Console.WriteLine("QC Service received and published message");

                await messageBus.PublishMessage(orderHeaderUpdateMessage, orderHeaderUpdateTopic);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in QC Service during received and published message");
                Console.WriteLine(ex.Message);
            }
        }

        private Task OnServiceBusException(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine("QC Service on service bus exception");
            Console.WriteLine(arg.Exception);
            return Task.CompletedTask;
        }

        public void stop()
        {
        }
    }
}
