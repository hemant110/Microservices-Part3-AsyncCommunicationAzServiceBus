using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Models
{
    public class Products
    {
        public string Product { get; set; }
        public string Product_Description { get; set; }
        public string Product_Group { get; set; }
        public string Active { get; set; }
        public string Unit { get; set; }
        public string Customer_Code { get; set; }

        public string Customer_Name { get; set; }

        public string Warehouse_Code { get; set; }

        public string Warehouse_Name { get; set; }

        public string Company_Code { get; set; }

        public string Company_Name { get; set; }
    }
}
