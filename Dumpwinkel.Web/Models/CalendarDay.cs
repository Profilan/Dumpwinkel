using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsToday { get; set; }
        public int MaxPersons { get; set; }

        public CalendarDay()
        {
            IsVisible = true;
            IsAvailable = false;
            IsToday = false;
            
        }
    }
}