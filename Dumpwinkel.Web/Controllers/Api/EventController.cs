using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dumpwinkel.Web.Controllers.Api
{

    public class EventController : ApiController
    {
        private readonly EventRepository _eventRepository = new EventRepository();

        [Route("api/event")]
        [HttpGet]
        public IHttpActionResult GetByDate(string date)
        {
            var items = _eventRepository.ListByDate(DateTime.Parse(date)).Where(x => x.MaximumNumberOfVisitors > 0);

            var events = new List<EventViewModel>();
            foreach (var eventItem in items)
            {
                events.Add(new EventViewModel()
                {
                    Id = eventItem.Id,
                    MaxPersons = eventItem.MaximumNumberOfVisitors,
                    StartTime = eventItem.TimeRange.Start.ToShortTimeString(),
                    EndTime = eventItem.TimeRange.End.ToShortTimeString()
                });
            }

            return Ok(events);
        }
    }
}
