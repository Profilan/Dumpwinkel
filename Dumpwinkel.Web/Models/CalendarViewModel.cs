using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class CalendarViewModel
    {
        public IList<CalendarMonth> CalendarMonths { get; set; }
    }
}