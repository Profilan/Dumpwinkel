namespace Dumpwinkel.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public string StartDate { get; set; }
        public double CancellationCount { get; set; }
        public double VisitCount { get; set; }
        public double ConfirmationCount { get; set; }
    }
}