using System;
using System.Collections.Generic;

namespace EventServer.Data.Entities
{
    public partial class QueueStatus
    {
        public Guid StatusId { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusMessage { get; set; }
        public int StatusIndicator { get; set; }
    }
}
