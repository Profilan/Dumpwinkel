using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dumpwinkel.Web.Areas.Admin.Controllers.Api
{
    public class CalendarController : ApiController
    {
        private readonly EventRepository _eventRepository = new EventRepository();

        [Route("api/calendar")]
        [HttpGet]
        public IHttpActionResult GetByDate(DateTime currentDate)
        {
            var firstDayOfTheMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfTheMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            DateTime startDate = firstDayOfTheMonth.AddDays(-(int)firstDayOfTheMonth.DayOfWeek + 1);

            var days = new List<CalendarDay>();
            for (int i = 0; i < 42; i++)
            {
                DateTime date = startDate.AddDays(i);

                var events = _eventRepository.ListByDate(date);
                if (events.Count() > 0 && date >= DateTime.Now)
                {
                    days.Add(new CalendarDay()
                    {
                        Date = date
                    });
                }
            }

            return Ok(days);
        }
    }
}
