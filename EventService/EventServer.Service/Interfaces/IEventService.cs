using EventServer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Server.Interfaces
{
    public interface IEventService
    {
        IQueryable GetEvents();
        IQueryable GetEventsByTopic(string topic);
        IQueryable GetEventDetail(Guid eventId);
        IQueryable GetProcessEvents();
        IQueryable GetProcessEventDetail(Guid processEventId);

        void UpdateEvent(Event eventDetail);
        void UpdateProcessEvent(ProcessEvent processEventDetail);        
    }
}
