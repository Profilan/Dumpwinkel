using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers.Admin
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly VisitorRepository _visitorRepository = new VisitorRepository();

        public ActionResult Index(string eventId = null)
        {
            IEnumerable<Registration> items;

            if (!String.IsNullOrEmpty(eventId))
            {
                var eventItem = _eventRepository.GetById(new Guid(eventId));
                items = _registrationRepository.GetByEvent(eventItem);
            }
            else
            {
                items = _registrationRepository.List();
            }

            var registrations = new List<RegistrationViewModel>();
            foreach (var registration in items)
            {
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);
                registrations.Add(new RegistrationViewModel()
                {
                    Name = visitor.Name,
                    Email = visitor.Email,
                    City = visitor.City,
                    EventId = eventItem.Id,
                    Title = eventItem.TimeRange.ToString(),
                    NumberOfVisitors = registration.NumberOfVisitors
                });
            }

            return View(registrations);
        }
    }
}