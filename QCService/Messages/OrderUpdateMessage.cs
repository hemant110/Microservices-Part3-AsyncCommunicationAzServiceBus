using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Integration.Messages;

namespace QCService.Messages
{
    public class OrderHeaderUpdateMessage : IntegrationBaseMessage
    {
        public string Order_Code { get; set; }
        public string Order_Status { get; set; }
    }
}
