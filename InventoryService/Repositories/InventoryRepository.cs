using InventoryService.DBContexts;
using InventoryService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DbContextOptions<InventoryDBContext> inventoryDBContext;
        public InventoryRepository(DbContextOptions<InventoryDBContext> inventoryDBContext)
        {
            this.inventoryDBContext = inventoryDBContext;
        }
        public async Task<Inventory> AddUpdateInventory(Inventory inventory)
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                var invExists = await _inventoryDBContext.Inventory.Where(x => x.Inventory_Tag == inventory.Inventory_Tag).FirstOrDefaultAsync();
                if (invExists != null)
                {
                    invExists.Inventory_Qty = inventory.Inventory_Qty == 0 ? invExists.Inventory_Qty : inventory.Inventory_Qty;
                    invExists.UpdatedBy = "AzService";
                    invExists.UpdatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    invExists.UpdatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                    invExists.Inventory_LockCode = inventory.Inventory_LockCode == null ? invExists.Inventory_LockCode : inventory.Inventory_LockCode;
                    invExists.Inventory_LockStatus = inventory.Inventory_LockStatus == null ? invExists.Inventory_LockStatus : inventory.Inventory_LockStatus;
                    invExists.Inventory_Location = inventory.Inventory_Location == null ? invExists.Inventory_Location : inventory.Inventory_Location;
                    _inventoryDBContext.Inventory.Update(invExists);
                    await _inventoryDBContext.SaveChangesAsync();
                    return invExists;
                }
                else
                {
                    inventory.CreatedBy = "Azure Service";
                    inventory.CreatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    inventory.CreatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                    inventory.Inventory_Location = "Receving Area";
                    inventory.Inventory_LockCode = "RCV";
                    inventory.Inventory_LockStatus = "UNLOCKED";
                    inventory.Inventory_Location = "Receiving Area";
                    inventory.Active = true;
                    inventory.IsDeleted = false;

                    _inventoryDBContext.Inventory.Add(inventory);
                    await _inventoryDBContext.SaveChangesAsync();
                    return inventory;
                }
            }
        }

        public async Task<IEnumerable<Inventory>> GetAll()
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                return await _inventoryDBContext.Inventory.ToListAsync();
            }
        }

        public async Task<Inventory> GetInventoryByProduct(string producCode)
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                return await _inventoryDBContext.Inventory.Where(x => x.Inventory_Product == producCode).FirstOrDefaultAsync();
            }
        }

        public async Task<Inventory> GetInventoryByTag(string inventoyTag)
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                return await _inventoryDBContext.Inventory.Where(x => x.Inventory_Tag == inventoyTag).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> InventoryExists(string inventoyTag)
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                return (await _inventoryDBContext.Inventory.AnyAsync(x => x.Inventory_Tag == inventoyTag));
            }
        }

        public async Task<bool> SaveChanges()
        {
            using (var _inventoryDBContext = new InventoryDBContext(inventoryDBContext))
            {
                return (await _inventoryDBContext.SaveChangesAsync() > 0);
            }
        }
    }
}
