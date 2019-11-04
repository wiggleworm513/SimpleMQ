using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Event
    {
        public Guid EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventSender { get; set; }
        public string EventTopic { get; set; }
        public string EventPayload { get; set; }
        public int EventStatus { get; set; }
    }
}
