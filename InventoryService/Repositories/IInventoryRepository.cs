using InventoryService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public interface IInventoryRepository
    {
        Task<bool> InventoryExists(string inventoyTag);
        Task<Inventory> GetInventoryByProduct(string producCode);
        Task<Inventory> GetInventoryByTag(string inventoyTag);
        Task<IEnumerable<Inventory>> GetAll();
        Task<Inventory> AddUpdateInventory(Inventory inventory);
        Task<bool> SaveChanges();

    }
}
