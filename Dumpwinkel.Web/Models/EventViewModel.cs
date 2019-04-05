using Dumpwinkel.Logic.Models;
using Dumpwinkel.Web.Areas.Admin.Models;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public int MaxPersons { get; set; }
        public int Registered { get; set; } 
        public int Available { get; set; }
        public int Pending { get; set; }
        public int Visited { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PublishUp { get; set; }
        public string ThemeTitle { get; set; }

        [Display(Name = "Thema")]
        public string ThemeId { get; set; }
        public IList<SelectListItem> Themes { get; set; }

        public int Amount { get; set; }
        public Unit Unit { get; set; }

        public EventViewModel()
        {
            MaxPersons = 200;
        }
    }
}