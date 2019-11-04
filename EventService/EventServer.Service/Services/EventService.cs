using EventServer.Data.Entities;
using EventServer.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventServer.Service.Services
{
    public class EventService : IEventService
    {
        private readonly EventQueueContext _context;        

        public EventService(EventQueueContext context)
        {
            _context = context;            
        }

        #region Query Methods

        public IQueryable GetEventDetail(Guid eventId)
        {
            IQueryable data = null;

            data = _context.Event
                    .Where(e => e.EventId == eventId);

            return data;
        }

        public IQueryable GetEvents()
        {
            IQueryable data = null;

            data = _context.Event;

            return data;
        }

        public IQueryable GetEventsByDate(DateTime date)
        {
            IQueryable data = null;

            data = _context.Event
                .Where(e => e.EventDate.ToShortDateString() == date.ToShortDateString());

            return data;
        }

        public IQueryable GetEventsByTopic(string topic)
        {
            IQueryable data = null;

            data = _context.Event
                .Where(e => e.EventTopic == topic
                         && e.EventStatus == 0);

            return data;
        }

        public IQueryable GetProcessEventDetail(Guid processEventId)
        {
            IQueryable data = null;

            data = _context.ProcessEvent
                    .Where(pe => pe.ProcessEventId == processEventId);

            return data;
        }

        public IQueryable GetProcessEvents()
        {
            IQueryable data = null;

            data = _context.ProcessEvent;

            return data;
        }

        public IQueryable GetLastQueueStatus()
        {
            IQueryable data = null;

            data = _context.QueueStatus.OrderByDescending(q => q.StatusDate).Take(1);

            return data;
        }
        public IQueryable GetAllQueueStatus(DateTime date)
        {
            IQueryable data = null;

            data = _context.QueueStatus.Where(q => q.StatusDate.ToShortDateString() == date.ToShortDateString());

            return data;
        }

        #endregion

        #region Create, Update, Delete Methods

        public void UpdateEvent(Event eventDetail)
        {
            Event updateEvent = eventDetail;

            if (updateEvent.EventId == Guid.Empty)
            {
                _context.Event.Add(updateEvent);
            }
            else
            {
                _context.Event.Attach(updateEvent);
            }

            _context.SaveChanges();
        }

        public void UpdateProcessEvent(ProcessEvent processEventDetail)
        {
            ProcessEvent updateProcessEvent = processEventDetail;

            if(updateProcessEvent.ProcessEventId == Guid.Empty)
            {
                _context.ProcessEvent.Add(updateProcessEvent);
            }
            else
            {
                _context.ProcessEvent.Attach(updateProcessEvent);
            }

            _context.SaveChanges();
        }

        public void AddQueueStatus(QueueStatus status)
        {
            _context.QueueStatus.Add(status);

            _context.SaveChanges();
        }

        #endregion
    }
}
