using EventServer.Data.Entities;
using EventServer.Server.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventServer.Server.Services
{
    public class EventBus : IEventBus, IHostedService
    {
        private bool connected = false;
        private SubscriberSocket socket;
        private INetMQPoller poller;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventBus> _logger;
        private readonly IServiceProvider _services;

        public EventBus(IConfiguration configuration, ILogger<EventBus> logger, IServiceProvider services)
        {
            _configuration = configuration;
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!connected)
            {
                socket = new SubscriberSocket(_configuration["NetMQ:SubscribeConnection"]);
                socket.Options.ReceiveHighWatermark = 1000;
                socket.Subscribe(_configuration["EventBus:Topic"]);
                socket.ReceiveReady += ReceieveMessage;
                poller = new NetMQPoller { socket };
                poller.Run();
                connected = true;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (connected)
            {
                socket.Unsubscribe(_configuration["EventBus:Topic"]);
                socket.Disconnect(_configuration["NetMq:SubscribeConnection"]);

                poller.Stop();

                connected = false;
            }

            return Task.CompletedTask;
        }

        public void ReceieveMessage(object sender, NetMQSocketEventArgs e)
        {
            //Get the message
            NetMQMessage message = e.Socket.ReceiveMultipartMessage();

            // Check the framecount for validation ?
            var frameCount = message.FrameCount;

            if (frameCount == 4)
            {
                var topic = message[0].ConvertToString();
                var date = DateTime.Parse(message[1].ConvertToString());
                var messageSender = message[2].ConvertToString();
                var payload = message[3].ConvertToString();

                _logger.LogInformation($"Receieved Message - Topic {topic} Date {date} Sender {messageSender} Payload {payload}");

                using (var scope = _services.CreateScope())
                {
                    IEventService service = scope.ServiceProvider.GetRequiredService<IEventService>();

                    Event receievedEvent = new Event()
                    {
                        EventDate = date,
                        EventSender = messageSender,
                        EventPayload = payload,
                        EventTopic = topic,
                        EventStatus = 0
                    };

                    service.UpdateEvent(receievedEvent);

                    if(receievedEvent.EventId != Guid.Empty)
                    {
                        _logger.LogInformation($"The event was saved with ID {receievedEvent.EventId}");
                    }
                    else
                    {
                        _logger.LogError("Unable to save the event");
                    }
                }
            }
        }
    }
}
