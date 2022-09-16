using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCService.MessageConsumer
{
    public interface IServiceBusConsumer
    {
        void start();
        void stop();
    }
}
