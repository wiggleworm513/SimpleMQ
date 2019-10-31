using System;
using System.Collections.Generic;

namespace EventServer.Data.Entities
{
    public partial class Event
    {
        public Guid EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventSender { get; set; }
        public string EventTopic { get; set; }
        public string EventPayload { get; set; }
        public int EventStatus { get; set; }
    }
}
