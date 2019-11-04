using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Service
{
    public static class NetMQHelper
    {
        private static NetMQMessage CreateMessage(string[] messageData)
        {
            NetMQMessage message = new NetMQMessage();

            foreach(var data in messageData)
            {
                message.Append(data);
            }

            return message;
        }

        public static NetMQMessage CreateMessage(string topic, string sender, string payload)
        {
            return CreateMessage(new string[] { topic, DateTime.Now.ToString(), sender, payload });
        }

        public static NetMQMessage CreateRepubMessage(string topic, string sender, string payload)
        {
            return CreateMessage(new string[] { $"Republish-{topic}", DateTime.Now.ToString(), sender, payload });
        }

        public static void SendMessage(string connection, NetMQMessage message)
        {
            using (var pubSocket = new PublisherSocket(connection))
            {   
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.SendMultipartMessage(message);
            }
        }
    }
}
