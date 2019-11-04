using EventServer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Service.Interfaces
{
    public interface IEventService
    {
        IQueryable GetEvents();
        IQueryable GetEventsByDate(DateTime date);
        IQueryable GetEventsByTopic(string topic);
        IQueryable GetEventDetail(Guid eventId);
        IQueryable GetProcessEvents();
        IQueryable GetProcessEventDetail(Guid processEventId);
        IQueryable GetLastQueueStatus();
        IQueryable GetAllQueueStatus(DateTime date);

        void UpdateEvent(Event eventDetail);
        void UpdateProcessEvent(ProcessEvent processEventDetail);
        void AddQueueStatus(QueueStatus status);
    }
}
