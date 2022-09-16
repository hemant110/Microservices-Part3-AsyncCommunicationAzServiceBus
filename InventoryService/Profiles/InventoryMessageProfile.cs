using AutoMapper;
using InventoryService.Entities;
using InventoryService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Profiles
{
    public class InventoryMessageProfile: Profile
    {
        public InventoryMessageProfile()
        {
            CreateMap<InventoryInsertMessage, Inventory>();
        }
    }
}
