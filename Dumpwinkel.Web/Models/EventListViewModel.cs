using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class EventListViewModel
    {
        public DateTime Date { get; set; }

        public int MaxPersons { get; set; }

        public CalendarMonth Month { get; set; }

        public IEnumerable<EventViewModel> Events;
    }
}