using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Dumpwinkel.Web.Controllers.Api
{

    public class EventController : ApiController
    {
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly DumpstoreRepository _dumpstoreRepository = new DumpstoreRepository();
        private readonly ThemeRepository _themeRepository = new ThemeRepository();

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
                int visitedCount = _registrationRepository.GetVisitedCount(eventItem);

                string themeTitle = "";
                if (eventItem.Theme != null)
                {
                    Theme theme = _themeRepository.GetById(eventItem.Theme.Id);
                    if (theme != null)
                    {
                        themeTitle = theme.Title.ToUpper();
                    }
                }
               
                events.Add(new EventViewModel()
                {
                    Id = eventItem.Id,
                    Registered = registeredCount,
                    MaxPersons = eventItem.MaximumNumberOfVisitors,
                    Available = eventItem.MaximumNumberOfVisitors - registeredCount,
                    Pending =  pendingCount,
                    StartTime = eventItem.TimeRange.Start.ToShortTimeString(),
                    EndTime = eventItem.TimeRange.End.ToShortTimeString(),
                    Visited = visitedCount,
                    ThemeTitle = themeTitle,
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

        [Route("api/event/before/{date}")]
        [HttpPost]
        public HttpResponseMessage Before(string date)
        {
            IEnumerable<Event> events = _eventRepository.ListByDate(DateTime.Parse(date));

            var dumpstore = _dumpstoreRepository.GetById(new Guid("B980E94C-4436-4966-B291-2B377080E6E3"));

            var firstEvent = events.First();

            var startTime = firstEvent.TimeRange.Start.Subtract(firstEvent.TimeRange.Duration);

            var eventItem = Event.Create(dumpstore, startTime, firstEvent.TimeRange.Start, firstEvent.MaximumNumberOfVisitors);
            _eventRepository.Insert(eventItem);

            return Request.CreateResponse(HttpStatusCode.OK, new { EventDate = date }, JsonMediaTypeFormatter.DefaultMediaType);
        }

        [Route("api/event/after/{date}")]
        [HttpPost]
        public HttpResponseMessage After(string date)
        {
            IEnumerable<Event> events = _eventRepository.ListByDate(DateTime.Parse(date));

            var dumpstore = _dumpstoreRepository.GetById(new Guid("B980E94C-4436-4966-B291-2B377080E6E3"));

            var lastEvent = events.Last();

            var endTime = lastEvent.TimeRange.End.Add(lastEvent.TimeRange.Duration);

            var eventItem = Event.Create(dumpstore, lastEvent.TimeRange.End, endTime, lastEvent.MaximumNumberOfVisitors);
            _eventRepository.Insert(eventItem);

            return Request.CreateResponse(HttpStatusCode.OK, new { EventDate = date }, JsonMediaTypeFormatter.DefaultMediaType);
        }

        [Route("api/event/delete/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var eventItem = _eventRepository.GetById(id);

                _eventRepository.Delete(id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Content(HttpStatusCode.NoContent, "Gebeurtenis is met succes verwijderd.");
        }

    }
}
