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
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string ThemeTitle { get; set; }
        public string ThemeDescription { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get;set; }

        [Required(ErrorMessage = "Plaats is verplicht")]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required(ErrorMessage = "E-mail adres is verplicht")]
        [Display(Name = "E-mail adres")]
        [EmailAddress(ErrorMessage = "E-mail adres is niet geldig")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Display(Name = "Bevestigd")]
        public bool Confirmed { get; set; }

        [Display(Name = "Bezocht")]
        public bool Visited { get; set; }

        [Required]
        [Display(Name = "Aantal personen (max 2)")]
        [Range(1, 2, ErrorMessage = "Kies een aantal tussen de {1} en {2}")]
        public int NumberOfVisitors { get; set; }

        public IList<ScanViewModel> Scans { get; set; }
    }
}