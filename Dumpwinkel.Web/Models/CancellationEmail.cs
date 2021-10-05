using Postal;

namespace Dumpwinkel.Web.Models
{
    public class CancellationEmail : Email
    {
        public string To { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTill { get; set; }
        public int NumberOfVisitors { get; set; }
        public string RegistrationId { get; set; }
        public string Disclaimer { get; set; }
        public string ThemeTitle { get; set; }
    }
}