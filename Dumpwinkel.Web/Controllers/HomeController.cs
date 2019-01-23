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
                    
                    var maxPersons = _eventRepository.GetMaxPersonsByDate(date);
                    days.Add(new Models.CalendarDay()
                    {
                        Date = date,
                        IsVisible = date < firstDayOfTheMonth || date > lastDayOfTheMonth ? false : true,
                        MaxPersons = _eventRepository.GetMaxPersonsByDate(date),
                        IsAvailable = maxPersons > 0 ? true : false,
                        IsPast = date < currentDate ? true : false
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
    }
}