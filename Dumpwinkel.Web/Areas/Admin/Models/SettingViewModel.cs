using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Models
{
    public class SettingViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Titel Kleur")]
        public string TitleColor { get; set; }

        [Required]
        [Display(Name = "Titel Grootte (px)")]
        public int TitleSize { get; set; }

        [Required]
        
        [Display(Name = "Introductie Tekst")]
        public string IntroText { get; set; }

        [Required]
        [Display(Name = "Introductie Tekst Kleur")]
        public string IntroTextColor { get; set; }

        [Required]
        [Display(Name = "Introductie Tekst Grootte")]
        public int IntroTextSize { get; set; }

        [Required]
        [Display(Name = "Achtergrond afbeelding")]
        public string BackgroundImageUrl { get; set; }

        public int MaxFileSize { get; set; }
        public string AcceptFileTypes { get; set; }
        public string UploadUrl { get; set; }
        public IList<string> Images { get; set; }

        [Display(Name = "Informatie tekst")]
        [AllowHtml]
        public string InfoText { get; set; }


        [Display(Name = "Disclaimer")]
        [AllowHtml]
        public string EmailDisclaimer { get; set; }

        public SettingViewModel()
        {
            Images = new List<string>();
        }
    }
}