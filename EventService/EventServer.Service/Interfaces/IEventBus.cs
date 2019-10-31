using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Server.Interfaces
{
    public interface IEventBus
    {
        void ReceieveMessage(object sender, NetMQSocketEventArgs e);
    }
}
