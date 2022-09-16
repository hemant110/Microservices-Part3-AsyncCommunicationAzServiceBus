using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderReceive.Models
{
    public class OrderLine
    {
        public string Order_Code { get; set; }

        public string Notes { get; set; }

        public string LineId { get; set; }

        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public string Lines_Unit { get; set; }

        public int QtyOrdered { get; set; }

        public int QtyPlanned { get; set; }
        public int QtyPicked { get; set; }
        public int QtyAllocated { get; set; }
        public Products Product { get; set; }
    }
}
