using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBeheerder,GRolDumpwinkelOrganisator")]
    public class DashboardController : Controller
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();

        public ActionResult Index(string startDate = null)
        {
            DateTime start, end;

            if (String.IsNullOrEmpty(startDate))
            {
                var currentDate = DateTime.Now;
                start = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                end = start.AddHours(24);
            }
            else
            {
                var currentDate = Convert.ToDateTime(startDate);
                start = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                end = start.AddHours(24);
            }

            var registrations = _registrationRepository.ListByDate(start, end);
            int registrationCount = registrations.Count();
            int confirmationCount = registrations.Where(x => x.Confirmed == true).Count();
            int visitedCount = registrations.Where(x => x.Visited == true).Count();
            int cancellationCount = registrations.Where(x => x.Cancelled == true).Count();

            DashboardViewModel viewModel = new DashboardViewModel
            {
                StartDate = start.ToString("yyyy-MM-dd"),
                ConfirmationCount = confirmationCount,
                VisitCount = visitedCount,
                CancellationCount = cancellationCount
            };

            return View(viewModel);
        }
    }
}