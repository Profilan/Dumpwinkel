using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Areas.Admin.Models
{
    public class ThemeViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }
    }
}