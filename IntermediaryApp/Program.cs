using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace IntermediaryApp
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            //Add Logging
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog("nlog.config");
            });


            services.AddTransient<Intermediary>();
            

            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();
            services.AddOptions();
            services.Configure<NetMQSettings>(options =>
            {
                options.PubAddress = config["NetMQ:PubAddress"];
                options.SubAddress = config["NetMQ:SubAddress"];
            });

            return services;
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                        
            try
            {
                var services = ConfigureServices();

                var servicesProvider = services.BuildServiceProvider();

                using (servicesProvider as IDisposable)
                {
                    var intermediary = servicesProvider.GetRequiredService<Intermediary>();
                    intermediary.Start();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                _logger.Error(ex, "Stopped program because of exception");
                throw;
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _logger.Warn("Pub/Sub Intermediary Processing is Shutting Down.");
            LogManager.Shutdown();
        }
    }
}
