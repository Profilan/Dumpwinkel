using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class EventListViewModel
    {
        public string Date { get; set; }

        public int MaxPersons { get; set; }

        public IEnumerable<EventViewModel> Events;
    }
}