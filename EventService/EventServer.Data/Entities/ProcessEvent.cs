using System;
using System.Collections.Generic;

namespace EventServer.Data.Entities
{
    public partial class ProcessEvent
    {
        public Guid ProcessEventId { get; set; }
        public Guid EventId { get; set; }
        public DateTime ProcessDate { get; set; }
        public int ProcessStatus { get; set; }
        public string ProcessMessage { get; set; }
    }
}
