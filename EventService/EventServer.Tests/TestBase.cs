using EventServer.Data.Entities;
using EventServer.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using EventServer.Server.Services;

namespace EventServer.Tests
{
    public class TestBase
    {
        protected IEventService _service;
        protected EventQueueContext _context;

        protected void Setup(string dbName)
        {
            _context = GetContextWithData(dbName);

            _service = new EventService(_context);
        }

        protected EventQueueContext GetContextWithData(string dbName)
        {
            var options = new DbContextOptionsBuilder<EventQueueContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new EventQueueContext(options);

            return context;
        }
    }
}
