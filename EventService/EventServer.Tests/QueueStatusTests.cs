using EventServer.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EventServer.Tests
{
    public class QueueStatusTests : TestBase
    {
        public QueueStatusTests()
        {
            Setup("queuestatus_tests");
        }

        [Fact]
        public void AddQueueStatus()
        {
            QueueStatus status = new QueueStatus() {
                 StatusDate = DateTime.Now,
                  StatusMessage = "Starting the Queue",
                  StatusIndicator = 1
            };

            _service.AddQueueStatus(status);

            Assert.NotNull(status);
            Assert.NotEqual(Guid.Empty, status.StatusId);
        }

        [Fact]
        public void GetLastQueueStatus(){

            QueueStatus status = new QueueStatus()
            {
                StatusDate = DateTime.Now,
                StatusMessage = "Ending the Queue",
                StatusIndicator = 0
            };

            _service.AddQueueStatus(status);

            Assert.NotNull(status);
            Assert.NotEqual(Guid.Empty, status.StatusId);

            var data = JsonConvert.SerializeObject(_service.GetLastQueueStatus());

            Assert.NotNull(data);
            Assert.NotEqual("", data);
            Assert.NotEqual("[]", data);

            QueueStatus lastStatus = JsonConvert.DeserializeObject<List<QueueStatus>>(data).FirstOrDefault();

            Assert.Equal(status.StatusId, lastStatus.StatusId);
            Assert.Equal(status.StatusMessage, lastStatus.StatusMessage);
        }

        [Fact]
        public void GetAllQueueStatus()
        {
            QueueStatus status = new QueueStatus()
            {
                StatusDate = DateTime.Now,
                StatusMessage = "Ending the Queue",
                StatusIndicator = 0
            };

            _service.AddQueueStatus(status);

            Assert.NotNull(status);
            Assert.NotEqual(Guid.Empty, status.StatusId);

            var data = JsonConvert.SerializeObject(_service.GetAllQueueStatus(DateTime.Now));

            Assert.NotNull(data);
            Assert.NotEqual("", data);
            Assert.NotEqual("[]", data);

            List<QueueStatus> dailyStatus = JsonConvert.DeserializeObject<List<QueueStatus>>(data);

            Assert.NotNull(dailyStatus);
            Assert.NotEmpty(dailyStatus);
        }
    }
}
