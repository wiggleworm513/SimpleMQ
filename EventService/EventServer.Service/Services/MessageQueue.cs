using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventServer.Data.Entities;
using EventServer.Server.Interfaces;
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

            if (token.IsCancellationRequested)
            {
                await Task.FromCanceled(token);
            }
            else
            {
                using (var scope = _services.CreateScope())
                {
                    var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();

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

                        //Build a NetMQMessage
                        NetMQMessage message = NetMQHelper.CreateMessage(new string[0]);

                        //Publish the event
                        NetMQHelper.SendMessage(_configuration["NetMQ:PublishConnection"], message);
                    }

                    //Wait for x milliseconds
                    Thread.Sleep(int.Parse(_configuration["MessageQueue:SleepTime"]));

                    //Process again
                    await ProcessQueue(token);
                }
            }
        }
    }
}
