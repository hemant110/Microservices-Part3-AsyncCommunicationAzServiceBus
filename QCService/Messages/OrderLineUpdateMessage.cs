using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Integration.Messages;

namespace QCService.Messages
{
    public class OrderLineUpdateMessage :IntegrationBaseMessage
    {
        public string Order_Tag { get; set; }
        public string Order_Code { get; set; }
        public int Qty_Passed { get; set; }
    }
}
