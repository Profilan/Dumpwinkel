using Dumpwinkel.Logic.Models;
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
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();

        [Route("api/event")]
        [HttpGet]
        public IHttpActionResult GetByDate(string date)
        {
            var items = _eventRepository.ListByDate(DateTime.Parse(date)).Where(x => x.MaximumNumberOfVisitors > 0);

            var events = new List<EventViewModel>();
            foreach (var eventItem in items)
            {
                int pendingCount = _registrationRepository.GetPendingCount(eventItem);
                int registeredCount = _registrationRepository.GetRegisteredCount(eventItem);
               
                events.Add(new EventViewModel()
                {
                    Id = eventItem.Id,
                    Registered = registeredCount,
                    MaxPersons = eventItem.MaximumNumberOfVisitors,
                    Available = eventItem.MaximumNumberOfVisitors - registeredCount,
                    Pending =  pendingCount,
                    StartTime = eventItem.TimeRange.Start.ToShortTimeString(),
                    EndTime = eventItem.TimeRange.End.ToShortTimeString()
                });
            }

            return Ok(events);
        }

        private int GetRegistered(Event eventItem)
        {
            var registrations = _registrationRepository.GetByEvent(eventItem).Where(x => x.Confirmed = true);

            return registrations.Count();
        }

        private int GetPending(Event eventItem)
        {
            var registrations = _registrationRepository.GetByEvent(eventItem).Where(x => x.Confirmed = false);

            return registrations.Count();
        }

    }
}
