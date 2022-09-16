using Warehouse.Integration.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderReceive.Entities;

namespace OrderReceive.Messages
{
    public class QCInsertMessage : IntegrationBaseMessage
    {
        public List<QCLineMessage> QualityCheckList { get; set; }
    }
}
