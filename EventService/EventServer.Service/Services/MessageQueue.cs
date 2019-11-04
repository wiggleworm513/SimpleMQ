using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventServer.Data.Entities;
using EventServer.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;


namespace EventServer.Service.Services
{
    public class MessageQueue : BackgroundService
    {
        private readonly ILogger<MessageQueue> _logger;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;


        public MessageQueue(ILogger<MessageQueue> logger, IServiceProvider services, IConfiguration configuration)
        {
            _logger = logger;
            _services = services;
            _configuration = configuration;
        }

        #region Overrides

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Service Executing ...");

            await ProcessQueue(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }

        #endregion

        private async Task ProcessQueue(CancellationToken token)
        {
            _logger.LogInformation("Processing Message Queue ...");

            using (var scope = _services.CreateScope())
            {
                var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                try
                {
                    eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 1, StatusMessage = "Queue Processing Started .." });

                    if (token.IsCancellationRequested)
                    {
                        eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 2, StatusMessage = "Cancellation Requested .." });
                        await Task.FromCanceled(token);
                    }
                    else
                    {
                        eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 1, StatusMessage = "Getting Events to Process .." });
                        //Get events to process
                        var eventsToProcess = JsonConvert.SerializeObject(eventService.GetEventsByTopic(_configuration["MessageQueue:Topic"]));

                        foreach (var messageEvent in JsonConvert.DeserializeObject<List<Event>>(eventsToProcess))
                        {
                            //Create a ProcessEvent for each messageEvent
                            ProcessEvent processEvent = new ProcessEvent()
                            {
                                EventId = messageEvent.EventId,
                                ProcessDate = DateTime.Now,
                                ProcessStatus = 0,
                                ProcessMessage = ""
                            };

                            //Save the process event
                            eventService.UpdateProcessEvent(processEvent);

                            string topic = messageEvent.EventTopic.Split('-', StringSplitOptions.RemoveEmptyEntries)[1];

                            //Build a NetMQMessage
                            NetMQMessage message = NetMQHelper.CreateMessage(topic, messageEvent.EventSender, messageEvent.EventPayload);

                            eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 1, StatusMessage = $"Sending Event for {messageEvent.EventId} .." });
                            //Publish the event
                            NetMQHelper.SendMessage(_configuration["NetMQ:PublishConnection"], message);
                        }
                        eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 0, StatusMessage = "Queue Processing Completed .." });
                        //Wait for x milliseconds
                        Thread.Sleep(int.Parse(_configuration["MessageQueue:SleepTime"]));

                        //Process again
                        await ProcessQueue(token);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "There was an error while the message queue was processing ");
                    eventService.AddQueueStatus(new QueueStatus() { StatusDate = DateTime.Now, StatusIndicator = 0, StatusMessage = "Queue Processing Has Crashed .." });
                    throw;
                }
            }
        }
    }
}
