using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class CalendarMonth
    {
        public string Title { get; set; }
        public IList<CalendarDay> CalendarDays { get; set; }
    }
}