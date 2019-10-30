using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pubSocket = new PublisherSocket(">tcp://127.0.0.1:5678"))
            {
                Console.WriteLine("Publisher socket connecting...");
                pubSocket.Options.SendHighWatermark = 1000;
                
                Thread.Sleep(1000);
                var msg = "This is a test message";

                Console.WriteLine("Sending message : {0}", msg);

                NetMQMessage message = new NetMQMessage(2);

                message.Append(new NetMQFrame("Test"));
                message.Append(new NetMQFrame(msg));

                pubSocket.SendMultipartMessage(message);                

                Console.WriteLine("Done");
            }
        }
    }
}
