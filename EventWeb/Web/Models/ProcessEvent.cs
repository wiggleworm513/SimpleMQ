using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ProcessEvent
    {
        public Guid ProcessEventId { get; set; }
        public Guid EventId { get; set; }
        public DateTime ProcessDate { get; set; }
        public int ProcessStatus { get; set; }
        public string ProcessMessage { get; set; }
    }
}
