using AutoMapper;
using InventoryService.Models;
using InventoryService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Integration.MessagingBus;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IMapper mapper;
        private readonly IMessageBus messageBus;
        private readonly string orderUpdateMessageTopic;
        private readonly IConfiguration configuration;
        public InventoryController(IInventoryRepository inventoryRepository, IMapper mapper, IMessageBus messageBus,
            IConfiguration configuration)
        {
            this.inventoryRepository = inventoryRepository;
            this.mapper = mapper;
            this.messageBus = messageBus;
            this.configuration = configuration;
            orderUpdateMessageTopic = this.configuration.GetValue<string>("OrderUpdateMessageTopic");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Inventory>>> Get()
        {
            var invList = await inventoryRepository.GetAll();
            return Ok(mapper.Map<IEnumerable<Inventory>>(invList));
        }

        [HttpGet("/api/[controller]/product/{productCode}", Name = "GetByProduct")]
        public async Task<ActionResult<IEnumerable<Models.Inventory>>> GetByProduct(string productCode)
        {
            var invList = await inventoryRepository.GetInventoryByProduct(productCode);
            return Ok(mapper.Map<IEnumerable<Inventory>>(invList));
        }

        [HttpGet("/api/[controller]/tag/{tag}", Name = "GetByTag")]
        public async Task<ActionResult<Models.Inventory>> GetByTag(string tag)
        {
            var invList = await inventoryRepository.GetInventoryByTag(tag);
            return Ok(mapper.Map<Inventory>(invList));
        }
    }
}
