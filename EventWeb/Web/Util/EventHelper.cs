using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Util
{
    public static class EventHelper
    {
        private static string baseUrl = "";

        public static void Configure(IConfiguration configuration)
        {
            baseUrl = configuration["EventService"];
        }

        public static List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();

            return events;
        }

        

        public static List<Event> GetEventsByDate(DateTime date)
        {
            List<Event> events = new List<Event>();

            var data = HttpClientHelper.GetServiceAsync($"{baseUrl}/api/Event/GetEventsByDate?date={date}");

            events = JsonConvert.DeserializeObject<List<Event>>(data);

            return events;
        }

        public static List<QueueStatus> GetQueueStatus(DateTime date)
        {
            List<QueueStatus> status = new List<QueueStatus>();

            var data = HttpClientHelper.GetServiceAsync($"{baseUrl}/api/Event/GetAllQueueStatus?date={date}");

            status = JsonConvert.DeserializeObject<List<QueueStatus>>(data);

            return status;
        }

        public static QueueStatus GetCurrentStatus()
        {
            QueueStatus status = new QueueStatus();

            var data = HttpClientHelper.GetServiceAsync($"{baseUrl}/api/Event/GetCurrenQueueStatus");

            status = JsonConvert.DeserializeObject<List<QueueStatus>>(data).FirstOrDefault();

            return status;
        }
            
    }
}
