using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class QueueStatus
    {
        public Guid StatusId { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusMessage { get; set; }
        public QueueStatusIndicator StatusIndicator { get; set; }
    }
}
