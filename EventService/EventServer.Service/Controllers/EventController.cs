using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventServer.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IEventService _service;

        public EventController(IEventService service, ILogger<EventController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            try
            {
                var data = _service.GetEvents();

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public IActionResult GetEventsByDate(DateTime date)
        {
            try
            {
                var data = _service.GetEventsByDate(date);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{topic}")]
        public IActionResult GetEventsByTopic(string topic)
        {
            try
            {
                var data = _service.GetEventsByTopic(topic);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{eventId}")]
        public IActionResult GetEventDetail(Guid eventId)
        {
            try
            {
                var data = _service.GetEventDetail(eventId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public IActionResult GetProcessEvents()
        {
            try
            {
                var data = _service.GetProcessEvents();

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{processEventId}")]
        public IActionResult GetProcessEventDetail(Guid processEventId)
        {
            try
            {
                var data = _service.GetProcessEventDetail(processEventId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public IActionResult GetCurrenQueueStatus()
        {
            try
            {
                var data = _service.GetLastQueueStatus();

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public IActionResult GetAllQueueStatus(DateTime date)
        {
            try
            {
                var data = _service.GetAllQueueStatus(date);

                return Ok(data);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "");
                return new StatusCodeResult(500);
            }
        }
    }
}
