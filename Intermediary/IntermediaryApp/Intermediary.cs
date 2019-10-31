using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IntermediaryApp
{
    public class Intermediary
    {
        private readonly ILogger<Intermediary> _logger;
        private readonly NetMQSettings _settings;

        public Intermediary(ILogger<Intermediary> logger, IOptions<NetMQSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public void Start()
        {
            using (var xpubSocket = new XPublisherSocket(_settings.PubAddress))
            using (var xsubSocket = new XSubscriberSocket(_settings.SubAddress))
            {
                // proxy messages between frontend / backend
                var proxy = new Proxy(xsubSocket, xpubSocket);

                _logger.LogInformation("Starting Pub/Sub Intermediary ...");
                _logger.LogInformation("Awaiting Connections ...");

                // blocks indefinitely
                proxy.Start();
            }
        }

    }
}
