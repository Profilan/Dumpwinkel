using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class ActivationEmail : Email
    {
        public string To { get; set; }
        public string ActivationUrl { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTill { get; set; }
        public int NumberOfVisitors { get; set; }
        public string ThemeTitle { get; set; }
    }
}