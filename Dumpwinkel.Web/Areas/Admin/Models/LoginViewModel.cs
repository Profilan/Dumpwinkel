using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
    }
}