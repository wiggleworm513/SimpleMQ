using EventServer.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EventServer.Tests
{
    public class EventTests : TestBase
    {
        public EventTests()
        {
            Setup("event_tests");
        }

        [Fact]
        public void AddEvent()
        {
            Event addedEvent = new Event() {
                 EventDate = DateTime.Now,
                  EventPayload = "This is a test",
                  EventSender = "UnitTest",
                  EventStatus = 0,
                  EventTopic = "Test"
            };

            _service.UpdateEvent(addedEvent);

            Assert.NotEqual(Guid.Empty, addedEvent.EventId);
        }

        [Fact]
        public void UpdateEvent()
        {
            Event addedEvent = new Event()
            {
                EventDate = DateTime.Now,
                EventPayload = "This is a test",
                EventSender = "UnitTest",
                EventStatus = 0,
                EventTopic = "Test"
            };

            _service.UpdateEvent(addedEvent);

            Assert.NotEqual(Guid.Empty, addedEvent.EventId);

            addedEvent.EventPayload = "This is a new payload";

            _service.UpdateEvent(addedEvent);

            Assert.NotEqual(Guid.Empty, addedEvent.EventId);

            Event foundEvent = JsonConvert.DeserializeObject<List<Event>>(JsonConvert.SerializeObject(_service.GetEventDetail(addedEvent.EventId))).FirstOrDefault();

            Assert.NotNull(foundEvent);
            Assert.Equal(addedEvent.EventId, foundEvent.EventId);
            Assert.Equal(addedEvent.EventPayload, foundEvent.EventPayload);
        }
    }
}
