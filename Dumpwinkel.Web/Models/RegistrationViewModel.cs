using Dumpwinkel.Logic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class RegistrationViewModel
    {
        public string Title { get; set; }

        [Display(Name = "Naam")]
        public string Name { get;set; }

        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Display(Name = "Email adres")]
        [EmailAddress]
        public string Email { get; set; }

        public Guid EventId { get; set; }
        

        [Display(Name = "Aantal personen (max 3)")]
        [Range(1, 3, ErrorMessage = "Kies een aantal tussen de {1} en {2}")]
        public int NumberOfVisitors { get; set; }
    }
}