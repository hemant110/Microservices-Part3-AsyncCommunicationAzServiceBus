using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QCService.Messages;
using QCService.Models;
using QCService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Integration.MessagingBus;

namespace QCService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityCheckController : ControllerBase
    {
        private readonly IQualityCheckRepository qualityCheckRepository;
        private readonly IMapper mapper;
        private readonly IMessageBus messageBus;
        private readonly IConfiguration configuration;
        private readonly string inventoryInsertTopic;
        private readonly string orderLineUpdateTopic;
        public QualityCheckController(IQualityCheckRepository qualityCheckRepository, IMapper mapper,
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            this.qualityCheckRepository = qualityCheckRepository;
            this.mapper = mapper;
            this.messageBus = messageBus;
            this.configuration = configuration;

            inventoryInsertTopic = this.configuration.GetValue<string>("InventoryUpdateTopic");
            orderLineUpdateTopic = this.configuration.GetValue<string>("OrderLineUpdateTopic");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QualityCheck>>> Get()
        {
            var qualityCheckList = await qualityCheckRepository.GetAllQualityCheckTasks();
            if (qualityCheckList.Count() <= 0)
            {
                return NotFound();
            }
            var qualityCheckModel = mapper.Map<List<QualityCheck>>(qualityCheckList.ToList());

            return Ok(qualityCheckModel);
        }
        [HttpGet("{orderCode}", Name = "GetQCByCode")]
        public async Task<ActionResult<IEnumerable<QualityCheck>>> GetByCode(string orderCode)
        {
            try
            {
                if (!await qualityCheckRepository.OrderExistsForQualityCheck(orderCode, String.Empty))
                {
                    return NotFound();
                }

                var qcList = await qualityCheckRepository.GetQualityCheckTasksByOrder(orderCode);

                var qcModel = mapper.Map<List<QualityCheck>>(qcList.ToList());

                return Ok(qcModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Something bad happens.");
            }
        }

        [HttpGet("{orderCode}/{qcTag}", Name = "GetQCByTag")]
        public async Task<ActionResult<QualityCheck>> GetByCodeAndTag(string orderCode, string qcTag)
        {
            if (!await qualityCheckRepository.OrderExistsForQualityCheck(orderCode, string.Empty))
            {
                return NotFound();
            }

            var qcList = await qualityCheckRepository.GetQualityCheckTasksByOrderAndTag(orderCode, qcTag);

            var qcModel = mapper.Map<QualityCheck>(qcList);

            return Ok(qcModel);
        }
        [HttpPut("{orderCode}/{qcTag}")]
        public async Task<ActionResult<QualityCheck>> Put(string orderCode, string qcTag,
            [FromBody] QualityCheckForUpdate qualityCheckForUpdate)
        {
            var qcExist = await qualityCheckRepository.GetQualityCheckTasksByOrderAndTag(orderCode, qcTag);

            if (qcExist == null)
            {
                return NotFound();
            }

            var qcEntity = mapper.Map<Entities.QualityCheck>(qualityCheckForUpdate);
            var processedQC = await qualityCheckRepository.UpdateQualityCheckStatus(orderCode, qcTag, qcEntity);
            await qualityCheckRepository.SaveChanges();
            var qcToReturn = mapper.Map<QualityCheck>(processedQC);

            InventoryInsertMessage inventoryInsertMessage = new InventoryInsertMessage()
            {
                Customer_Code = String.IsNullOrEmpty(qcEntity.Customer_Code) == true ? "ABC" : qcEntity.Customer_Code,
                Customer_Name = String.IsNullOrEmpty(qcEntity.Customer_Name) == true ? "ABC" : qcEntity.Customer_Name,
                Warehouse_Code = String.IsNullOrEmpty(qcEntity.Warehouse_Code) == true ? "ABC" : qcEntity.Warehouse_Code,
                Warehouse_Name = String.IsNullOrEmpty(qcEntity.Warehouse_Name) == true ? "ABC" : qcEntity.Warehouse_Name,
                Company_Code = String.IsNullOrEmpty(qcEntity.Company_Code) == true ? "ABC" : qcEntity.Company_Code,
                Company_Name = String.IsNullOrEmpty(qcEntity.Company_Name) == true ? "ABC" : qcEntity.Company_Name,
                Inventory_CreationDate = String.IsNullOrEmpty(qcEntity.CreatedDate) == true ? DateTime.Now.Date.ToString("yyyy-MM-dd") : qcEntity.CreatedDate,
                Inventory_CreationTime = String.IsNullOrEmpty(qcEntity.CreatedTime) == true ? DateTime.Now.TimeOfDay.ToString().Substring(0, 8) : qcEntity.CreatedTime,
                Inventory_Order = qcToReturn.QC_List,
                Inventory_Tag = qcToReturn.QC_Tag,
                Inventory_Product = qcToReturn.Product_Code,
                Inventory_Qty = qcToReturn.Qty_Passed,
                Inventory_ProductDescription = qcToReturn.Product_Code,
                Inventory_oLineId = qcToReturn.QualityCheckId.ToString()
            };

            try
            {
                await messageBus.PublishMessage(inventoryInsertMessage, inventoryInsertTopic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            OrderLineUpdateMessage oLineUpdateMessage = new OrderLineUpdateMessage()
            {
                Order_Code = qcEntity.QC_List,
                Order_Tag = qcEntity.QC_Tag,
                Qty_Passed = qcEntity.Qty_Passed
            };

            try
            {
                await messageBus.PublishMessage(oLineUpdateMessage, orderLineUpdateTopic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return CreatedAtRoute("GetQCByTag", new { orderCode = qcToReturn.QC_List, qcTag = qcToReturn.QC_Tag }, qcToReturn);
        }

        [HttpPost("{orderCode}")]
        public async Task<ActionResult<QualityCheck>> Post(string orderCode, [FromBody] List<QualityCheckForCreation> qualityCheckForInsert)
        {
            try
            {
                int i = 1;
                List<QualityCheck> qcList = new List<QualityCheck>();
                foreach (var qcForInsert in qualityCheckForInsert)
                {
                    string qcTag = orderCode + "-" + qcForInsert.QC_ListDate + "-" + i.ToString();
                    qcForInsert.QC_List = orderCode;
                    qcForInsert.QC_Tag = qcTag;
                    qcForInsert.QualityCheckId = Guid.NewGuid();
                    i++;

                    var qualityCheck = await qualityCheckRepository.OrderExistsForQualityCheck(orderCode, qcTag);
                    if (qualityCheck)
                    {
                        return BadRequest("Record already exists.");
                    }

                    var qualityCheckEntity = mapper.Map<Entities.QualityCheck>(qcForInsert);
                    var qualityCheckProcessed = await qualityCheckRepository.UpdateQualityCheckStatus(orderCode, qcTag, qualityCheckEntity);
                    await qualityCheckRepository.SaveChanges();
                    var qualityCheckModel = mapper.Map<QualityCheck>(qualityCheckProcessed);
                    qcList.Add(qualityCheckModel);
                }

                return CreatedAtRoute("GetQCByCode", new { orderCode = orderCode }, qcList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Something bad happens.");
            }
        }
    }
}
