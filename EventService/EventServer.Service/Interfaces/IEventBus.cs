using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Service.Interfaces
{
    public interface IEventBus
    {
        void ReceieveMessage(object sender, NetMQSocketEventArgs e);
    }
}
