using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Dumpwinkel.Web.Controllers
{
   
    public class HomeController : BaseController
    {
        protected string[] Months = { "Januari", "Februari", "Maart", "April", "Mei", "Juni", "Juli", "Augustus", "September", "Oktober", "November", "December" };

        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly ThemeRepository _themeRepository = new ThemeRepository();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var currentDate = DateTime.Now;

            var months = new List<CalendarMonth>();
            for (int i = 0; i < 3; i++)
            {
                var getDate = currentDate.AddMonths(i);
                var firstDayOfTheMonth = new DateTime(getDate.Year, getDate.Month, 1);
                var lastDayOfTheMonth = new DateTime(getDate.Year, getDate.Month, DateTime.DaysInMonth(getDate.Year, getDate.Month));

                DateTime startDate = firstDayOfTheMonth.AddDays(-(int)firstDayOfTheMonth.DayOfWeek + 1);

                var days = new List<Models.CalendarDay>();
                for (int j = 0; j < 42; j++)
                {
                    DateTime date = startDate.AddDays(j);

#if DEBUG
                    var closingTime = new DateTime(date.Year, date.Month, date.Day).AddHours(24);
#else
                    var closingTime = new DateTime(date.Year, date.Month, date.Day).AddHours(12);
#endif

                   

                    var maxPersons = _eventRepository.GetMaxPersonsByDate(date);
                    var published = false;
                    var full = true;
                    if (maxPersons > 0)
                    {
                        var events = _eventRepository.ListByDate(date);
                        if (events.LastOrDefault().PublishUp < DateTime.Now)
                        {
                            published = true;
                        }
                        // check if the day is full by traversing all time slotes
                        foreach (var eventItem in events)
                        {
                            var count = GetRegisteredCountByEvent(eventItem);
                            if (count < eventItem.MaximumNumberOfVisitors)
                            {
                                full = false;
                            }
                        }
                    }

                    

                    // var registeredCount = GetRegisteredCount(date);
                    var themeDescription = GetThemeDescription(date);
                    
                    days.Add(new Models.CalendarDay()
                    {
                        Date = date,
                        IsVisible = date < firstDayOfTheMonth || date > lastDayOfTheMonth ? false : true,
                        MaxPersons = maxPersons,
                        IsAvailable = maxPersons > 0,
                        IsPast = date < new DateTime(currentDate.Year, currentDate.Month, currentDate.Day) || currentDate >= closingTime,
                        //IsFull = registeredCount >= maxPersons,
                        IsFull = full,
                        IsClosed = false,
                        IsPublished = published,
                        ThemeDescription = themeDescription
                    });

                }

                months.Add(new CalendarMonth()
                {
                    Title =   Months[getDate.Month - 1] + " " + getDate.Year, 
                    CalendarDays = days
                });
            }

            var viewModel = new CalendarViewModel()
            {
                CalendarMonths = months
            };

            return View(viewModel);
        }

        private int GetRegisteredCount(DateTime date)
        {
            var events =_eventRepository.ListByDate(date);

            int total = 0;
            foreach (var eventItem in events)
            {
                int count = _registrationRepository.GetRegisteredCount(eventItem);
                total += count;
            }

            return total;
        }

        private int GetRegisteredCountByEvent(Event eventItem)
        {
            int count = _registrationRepository.GetRegisteredCount(eventItem);

            return count;
        }

        private string GetThemeDescription(DateTime date)
        {
            var events = _eventRepository.ListByDate(date);

            Theme theme = null;
            if (events.Count() > 0)
            {
                if (events.LastOrDefault().Theme != null)
                {
                    theme = _themeRepository.GetById(events.LastOrDefault().Theme.Id);
                    if (theme != null)
                    {
                        return "<h5>" + theme.Title.ToUpper() + "</h5><p>" + theme.Description + "</p>";
                    }
                }
                return "<h5>Alle thema's</h5>";
            }

            return "";
        }
    }
}