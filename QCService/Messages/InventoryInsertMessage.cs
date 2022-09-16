using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Integration.Messages;

namespace QCService.Messages
{
    public class InventoryInsertMessage : IntegrationBaseMessage
    {
        public string Inventory_Tag { get; set; }
        public string Inventory_CreationDate { get; set; }
        public string Inventory_CreationTime { get; set; }
        public string Inventory_oLineId { get; set; }
        public string Inventory_Product { get; set; }
        public string Inventory_ProductDescription { get; set; }
        public int Inventory_Qty { get; set; }
        public string Inventory_Order { get; set; }
        public string Customer_Code { get; set; }
        public string Customer_Name { get; set; }
        public string Warehouse_Code { get; set; }
        public string Warehouse_Name { get; set; }
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
    }
}
