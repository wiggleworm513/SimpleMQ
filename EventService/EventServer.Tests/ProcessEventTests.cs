using EventServer.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EventServer.Tests
{
    public class ProcessEventTests : TestBase
    {
        public ProcessEventTests()
        {
            Setup("processevent_tests");
        }

        [Fact]
        public void AddProcessEvent()
        {
            ProcessEvent addedProcessEvent = new ProcessEvent()
            {
                EventId = Guid.NewGuid(),
                ProcessDate = DateTime.Now,
                ProcessStatus = 0,
                ProcessMessage = "There was an error processing the event"
            };

            _service.UpdateProcessEvent(addedProcessEvent);

            Assert.NotEqual(Guid.Empty, addedProcessEvent.ProcessEventId);
        }

        [Fact]
        public void UpdateProcessEvent()
        {
            ProcessEvent addedProcessEvent = new ProcessEvent()
            {
                EventId = Guid.NewGuid(),
                ProcessDate = DateTime.Now,
                ProcessStatus = 0,
                ProcessMessage = "There was an error processing the event"
            };

            _service.UpdateProcessEvent(addedProcessEvent);

            Assert.NotEqual(Guid.Empty, addedProcessEvent.ProcessEventId);

            addedProcessEvent.ProcessMessage = "";
            addedProcessEvent.ProcessStatus = 1;

            _service.UpdateProcessEvent(addedProcessEvent);

            Assert.NotEqual(Guid.Empty, addedProcessEvent.ProcessEventId);

            ProcessEvent foundProcessEvent = JsonConvert.DeserializeObject<List<ProcessEvent>>(JsonConvert.SerializeObject(_service.GetProcessEventDetail(addedProcessEvent.ProcessEventId))).FirstOrDefault();

            Assert.NotNull(foundProcessEvent);
            Assert.Equal(addedProcessEvent.ProcessEventId, foundProcessEvent.ProcessEventId);
            Assert.Equal(addedProcessEvent.EventId, foundProcessEvent.EventId);
            Assert.Equal(addedProcessEvent.ProcessStatus, foundProcessEvent.ProcessStatus);
            Assert.Equal(addedProcessEvent.ProcessMessage, foundProcessEvent.ProcessMessage);

        }
    }
}
