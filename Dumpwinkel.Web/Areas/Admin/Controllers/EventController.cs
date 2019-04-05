using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Areas.Admin.Models;
using Dumpwinkel.Web.Models;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBeheerder")]
    public class EventController : Controller
    {
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly DumpstoreRepository _dumpstoreRepository = new DumpstoreRepository();
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly ThemeRepository _themeRepository = new ThemeRepository();
        protected string[] Months = { "Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli", "Augustus", "September", "Oktober", "November", "December" };

        // GET: Event
        public ActionResult Index(string date = null)
        {
            var currentDate = DateTime.Now;
            if (!String.IsNullOrEmpty(date))
            {
                currentDate = DateTime.Parse(date);
            }

            var firstDayOfTheMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfTheMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            DateTime startDate = firstDayOfTheMonth.AddDays(-(int)firstDayOfTheMonth.DayOfWeek + 1);

            var days = new List<CalendarDay>();
            for (int j = 0; j < 42; j++)
            {
                DateTime day = startDate.AddDays(j);

                var maxPersons = _eventRepository.GetMaxPersonsByDate(day);
                days.Add(new CalendarDay()
                {
                    Date = day,
                    IsVisible = day >= firstDayOfTheMonth && day <= lastDayOfTheMonth,
                    MaxPersons = _eventRepository.GetMaxPersonsByDate(day),
                    IsAvailable = maxPersons > 0,
                    IsPast = day < currentDate,
                    IsToday = day.ToShortDateString() == DateTime.Now.ToShortDateString()
                });

            }

            var calendarMonth = new CalendarMonth()
            {
                Title = Months[currentDate.Month - 1] + " " + currentDate.Year,
                CalendarDays = days
            };

            IEnumerable<Event> items = _eventRepository.ListByDate(currentDate);

            var events = new List<EventViewModel>();
            foreach (var eventItem in items)
            {
                events.Add(new EventViewModel()
                {
                    Id = eventItem.Id,
                    StartTime = eventItem.TimeRange.Start.ToString("hh:mm"),
                    EndTime = eventItem.TimeRange.End.ToString("hh:mm"),
                    MaxPersons = eventItem.MaximumNumberOfVisitors,
                    Visited = _registrationRepository.GetVisitedCount(eventItem)
                });
            }

            var listViewModel = new EventListViewModel()
            {
                Date = currentDate,
                Events = events,
                Month = calendarMonth
            };

            return View(listViewModel);
        }

        public ActionResult Create(string date)
        {
            var dateNow = DateTime.Parse(date);

            var themeList = _themeRepository.List();
            var themes = new List<SelectListItem>();
            
            foreach (var theme in themeList)
            {
                themes.Add(new SelectListItem()
                {
                    Value = theme.Id.ToString(),
                    Text = theme.Title
                });
            }

            var viewModel = new EventViewModel()
            {
                StartTime = dateNow.ToString("yyyy-MM-ddT10:00" ),
                EndTime = dateNow.ToString("yyyy-MM-ddT12:00"),
                PublishUp = dateNow.ToString("yyyy-MM-ddThh:mm"),
                Unit = Profilan.SharedKernel.Unit.Minutes,
                Amount = 120,
                Themes = themes,
                ThemeId = "e0614191-56e9-41e3-99be-dcb5b7fedd64"
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var dumpstore = _dumpstoreRepository.GetById(new Guid("B980E94C-4436-4966-B291-2B377080E6E3"));
            var theme = _themeRepository.GetById(new Guid(collection["ThemeId"]));

            var startDate = DateTime.Parse(collection["StartTime"]);
            var endDate = DateTime.Parse(collection["EndTime"]);

            var maxPersonsPerHour = Convert.ToInt32(collection["MaxPersons"]);
            var interval = new Interval(Convert.ToInt32(collection["Amount"]), (Unit)Enum.Parse(typeof(Unit), collection["Unit"]));
            var publishUp = DateTime.Parse(collection["PublishUp"]);

            var events = Event.CreateRange(dumpstore, startDate, endDate, interval, maxPersonsPerHour, publishUp, theme);

            foreach (var newEvent in events)
            {
                _eventRepository.Insert(newEvent);
            }

            Request.Flash("success", "Evenementen zijn opgeslagen");

            return RedirectToAction("Index", new { date = startDate.ToString("yyyy-MM-dd") });
        }

        public ActionResult Edit(Guid id)
        {
            var eventItem = _eventRepository.GetById(id);

            var themeList = _themeRepository.List();
            var themes = new List<SelectListItem>();
            foreach (var theme in themeList)
            {
                themes.Add(new SelectListItem()
                {
                    Value = theme.Id.ToString(),
                    Text = theme.Title
                });
            }

            var viewModel = new EventViewModel()
            {
                Id = eventItem.Id,
                MaxPersons = eventItem.MaximumNumberOfVisitors,
                StartTime = eventItem.TimeRange.Start.ToString("yyyy-MM-ddTHH:mm"),
                EndTime = eventItem.TimeRange.End.ToString("yyyy-MM-ddTHH:mm"),
                PublishUp = eventItem.PublishUp.ToString("yyyy-MM-ddTHH:mm"),
                ThemeId = eventItem.Theme != null ? eventItem.Theme.Id.ToString() : "",
                Themes = themes
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var eventItem = _eventRepository.GetById(new Guid(collection["Id"]));

            try
            {
                Theme theme = null;
                if (!String.IsNullOrEmpty(collection["ThemeId"]))
                {
                    theme = _themeRepository.GetById(new Guid(collection["ThemeId"]));
                }

                eventItem.Theme = theme;
                eventItem.PublishUp =  DateTime.Parse(collection["PublishUp"]);

                eventItem.UpdateMaximumNumberOfVisitors(Convert.ToInt32(collection["MaxPersons"]));

                _eventRepository.Update(eventItem);

                Request.Flash("success", "Evenement is gewijzigd");

                return RedirectToAction("Index", "Event", new { date = eventItem.TimeRange.Start.ToString("yyyy-MM-dd") });
            }
            catch (Exception e)
            {
                Request.Flash("error", "Er is een ernstige fout opgetreden: " + e.Message);
                return RedirectToAction("Index", "Event", new { date = eventItem.TimeRange.Start.ToString("yyyy-MM-dd") });
            }
        }
    }
}