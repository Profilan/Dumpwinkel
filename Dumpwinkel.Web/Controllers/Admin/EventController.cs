using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers.Admin
{
    public class EventController : Controller
    {
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly DumpstoreRepository _dumpstoreRepository = new DumpstoreRepository();

        // GET: Event
        public ActionResult Index(string date = null)
        {
            if (String.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            IEnumerable<Event> items = _eventRepository.ListByDate(Convert.ToDateTime(date));

            var events = new List<EventViewModel>();
            foreach (var eventItem in items)
            {
                events.Add(new EventViewModel()
                {
                    Id = eventItem.Id,
                    StartTime = eventItem.TimeRange.Start.ToString("hh:mm"),
                    EndTime = eventItem.TimeRange.End.ToString("hh:mm"),
                    MaxPersons = eventItem.MaximumNumberOfVisitors

                });
            }

            var listViewModel = new EventListViewModel()
            {
                Date = date,
                Events = events
            };

            return View(listViewModel);
        }

        public ActionResult Create()
        {
            var dateNow = DateTime.Now;

            var viewModel = new EventViewModel()
            {
                StartTime = dateNow.ToString("yyyy-MM-ddT08:30" ),
                EndTime = dateNow.ToString("yyyy-MM-ddT17:00"),
                Unit = Profilan.SharedKernel.Unit.Minutes,
                Amount = 60
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var dumpstore = _dumpstoreRepository.GetById(new Guid("B980E94C-4436-4966-B291-2B377080E6E3"));

            var startDate = DateTime.Parse(collection["StartTime"]);
            var endDate = DateTime.Parse(collection["EndTime"]);

            var maxPersonsPerHour = Convert.ToInt32(collection["MaxPersons"]);
            var interval = new Interval(Convert.ToInt32(collection["Amount"]), (Unit)Enum.Parse(typeof(Unit), collection["Unit"]));

            var events = Event.CreateRange(dumpstore, startDate, endDate, interval, maxPersonsPerHour);

            foreach (var newEvent in events)
            {
                _eventRepository.Insert(newEvent);
            }


            return RedirectToAction("Index");
        }
    }
}