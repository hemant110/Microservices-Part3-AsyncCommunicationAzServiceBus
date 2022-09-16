using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Models
{
    public class Inventory
    {
        public string Inventory_Tag { get; set; }
        public string Inventory_CreationDate { get; set; }
        public string Inventory_CreationTime { get; set; }
        public string Inventory_oLineId { get; set; }
        public string Inventory_Product { get; set; }
        
        public int Inventory_Qty { get; set; }
        
        public string Inventory_Location { get; set; }
        
        public string Inventory_LockStatus { get; set; }
        
        public string Inventory_LockCode { get; set; }
        
        public string Inventory_Order { get; set; }
        
        public bool Active { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public string Customer_Code { get; set; }
        
        public string Customer_Name { get; set; }
        
        public string Warehouse_Code { get; set; }
        
        public string Warehouse_Name { get; set; }
        
        public string Company_Code { get; set; }
        
        public string Company_Name { get; set; }
    }
}
