using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public int MaxPersons { get; set; }
        public int Registered { get; set; } 
        public int Available { get; set; }
        public int Pending { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int Amount { get; set; }
        public Unit Unit { get; set; }

        public EventViewModel()
        {
            MaxPersons = 200;
        }
    }
}