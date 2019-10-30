using NetMQ;
using NetMQ.Sockets;
using System;

namespace SubscriberApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string topic = "Test"; // one of "TopicA" or "TopicB"
            using (var subSocket = new SubscriberSocket(">tcp://127.0.0.1:1234"))
            {
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Subscribe(topic);
                Console.WriteLine("Subscriber socket connecting...");
                while (true)
                {
                    NetMQMessage message = subSocket.ReceiveMultipartMessage();

                    if (message.FrameCount == 2)
                    {
                        string messageTopicReceived = message[0].ConvertToString();
                        string messageReceived = message[1].ConvertToString();
                        Console.WriteLine(messageReceived);
                    }
                    else
                    {
                        Console.WriteLine($"Receieved Message with {message.FrameCount} frames but expected only 2 frames.");
                    }
                }
            }
        }
    }
}
