using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderReceive.Messages;
using OrderReceive.Models;
using OrderReceive.Repositories;
using OrderReceive.Services;
using Polly.CircuitBreaker;
using Warehouse.Integration.MessagingBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OrderReceive.Entities;

namespace OrderReceive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderHeaderRepository orderHeaderRepository;
        private readonly IMapper mapper;
        private readonly IMessageBus messageBus;
        private readonly IQualityCheckService qualityCheckService;
        private readonly string qcInsertMessageTopic;
        private readonly IConfiguration configuration;
        public OrdersController(IOrderHeaderRepository orderHeaderRepository, IMapper mapper, IMessageBus
             messageBus, IQualityCheckService qualityCheckService, IConfiguration configuration)
        {
            this.orderHeaderRepository = orderHeaderRepository;
            this.mapper = mapper;
            this.messageBus = messageBus;
            this.qualityCheckService = qualityCheckService;
            this.configuration = configuration;
            qcInsertMessageTopic = this.configuration.GetValue<string>("QCInsertMessageTopic");
        }

        [HttpGet("{orderCode}", Name = "GetOrder")]
        public async Task<ActionResult<Models.OrderHeader>> Get(string orderCode)
        {
            var order = await orderHeaderRepository.GetOrderByID(orderCode);
            if (order == null)
            {
                return NotFound();
            }

            var result = mapper.Map<Models.OrderHeader>(order);
            result.Order_NoOfLines = order.OrderLines.Count;
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Models.OrderHeader>> Post(OrderForCreation orderForCreation)
        {
            var orderEntity = mapper.Map<Entities.OrderHeader>(orderForCreation);
            orderHeaderRepository.AddOrder(orderEntity);
            await orderHeaderRepository.SaveChanges();

            var orderToReturn = mapper.Map<Models.OrderHeader>(orderEntity);

            return CreatedAtRoute("GetOrder", new { orderCode = orderEntity.Order_Code }, orderToReturn);


        }

        [HttpPost("createorder")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderCreated orderCreated)
        {
            try
            {
                var orderExists = await orderHeaderRepository.GetOrderByID(orderCreated.Order_Code);

                if (orderExists == null)
                {
                    return BadRequest();
                }

                //update orderHeader status in DB
                Entities.OrderHeader orderHeader = await orderHeaderRepository.GetOrderByID(orderCreated.Order_Code);

                if (orderHeader != null)
                {

                    orderHeader.Order_Status = nameof(OrderStatusEnum.SentToQC);
                    orderHeaderRepository.AddOrder(orderHeader);
                    await orderHeaderRepository.SaveChanges();

                    // code changed from sync to async communication 
                    QCInsertMessage qCInsertMessage = new QCInsertMessage();
                    qCInsertMessage.QualityCheckList = new List<QCLineMessage>();
                    foreach (OrderLine oLine in orderCreated.OrderLines)
                    {
                        Messages.QCLineMessage qcCheck = new Messages.QCLineMessage
                        {
                            Product_Code = oLine.ProductCode,
                            QC_List = oLine.Order_Code,
                            QC_ListDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                            QC_ListTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8),
                            QC_Tag = oLine.LineId,
                            Active = true,
                            Company_Code = orderCreated.Company_Code,
                            CreatedBy = "Sys",
                            CreatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                            CreatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8),
                            Customer_Code = orderCreated.Customer_Code,
                            IsDeleted = false,
                            Warehouse_Code = orderCreated.Warehouse_Code

                        };
                        qCInsertMessage.QualityCheckList.Add(qcCheck);
                    }

                    try
                    {
                        await messageBus.PublishMessage(qCInsertMessage, qcInsertMessageTopic);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    return Accepted(qCInsertMessage);
                }
                else
                    return NotFound($"Order Header With OrderCode - {orderCreated.Order_Code} Not Found.");
            }
            catch (BrokenCircuitException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.StackTrace);
            }
        }

    }
}
