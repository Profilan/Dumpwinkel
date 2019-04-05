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
        public bool IsPast { get; set; }
        public bool IsFull { get; set; }
        public bool IsPublished { get; set; }
        public bool IsClosed { get; set; }
        public int MaxPersons { get; set; }
        public string ThemeDescription { get; set; }

        public CalendarDay()
        {
            IsVisible = true;
            IsAvailable = false;
            IsToday = false;
            IsPast = false;
            IsFull = false;
            IsClosed = false;
        }
    }
}