using Warehouse.Integration.Messages;
using System.Collections.Generic;

namespace QCService.Messages
{
    public class QCInsertMessage : IntegrationBaseMessage
    {
        public List<QCLineMessage> QualityCheckList { get; set; }
    }
}
