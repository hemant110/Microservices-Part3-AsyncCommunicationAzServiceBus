using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Entities
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        [Required]
        public string Inventory_Tag { get; set; }
        [Required]
        public string Inventory_CreationDate { get; set; }
        public string Inventory_CreationTime { get; set; }
        public string Inventory_oLineId { get; set; }
        [Required]
        public string Inventory_Product { get; set; }
        public string Inventory_ProductDescription { get; set; }
        [Required]
        public int Inventory_Qty { get; set; }
        [Required]
        public string Inventory_Location { get; set; }
        [Required]
        public string Inventory_LockStatus { get; set; }
        [Required]
        public string Inventory_LockCode { get; set; }
        [Required]
        public string Inventory_Order { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public string Customer_Code { get; set; }
        [Required]
        public string Customer_Name { get; set; }
        [Required]
        public string Warehouse_Code { get; set; }
        [Required]
        public string Warehouse_Name { get; set; }
        [Required]
        public string Company_Code { get; set; }
        [Required]
        public string Company_Name { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CreatedDate { get; set; }
        [Required]
        public string CreatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public string UpdatedTime { get; set; }

        public string DeletedBy { get; set; }

        public string DeletedDate { get; set; }

        public string DeletedTime { get; set; }
    }
}
