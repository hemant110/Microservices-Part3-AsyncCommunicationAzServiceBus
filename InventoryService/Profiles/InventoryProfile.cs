using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Profiles
{
    public class InventoryProfile :Profile
    {
        public InventoryProfile()
        {
            CreateMap<Entities.Inventory, Models.Inventory>().ReverseMap();
        }
    }
}
