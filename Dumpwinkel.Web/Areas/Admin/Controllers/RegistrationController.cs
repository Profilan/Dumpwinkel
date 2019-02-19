using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBeheerder")]
    public class RegistrationController : Controller
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly VisitorRepository _visitorRepository = new VisitorRepository();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string startDate = null, string eventId = null, string state = "all")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TimeRangeSortParm = String.IsNullOrEmpty(sortOrder) ? "timerange" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 25;
            int pageNumber = (page ?? 1);

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

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");

            var items = _registrationRepository.List(sortOrder, searchString, start, end, eventId, state);

            var registrations = new List<RegistrationViewModel>();
            foreach (var registration in items)
            {
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);
                var date = eventItem.TimeRange.Start.ToString("yyyy-MM-dd");
                var timeFrom = eventItem.TimeRange.Start.ToShortTimeString();
                var timeTill = eventItem.TimeRange.End.ToShortTimeString();
                registrations.Add(new RegistrationViewModel()
                {
                    Id = registration.Id,
                    Name = visitor.Name,
                    Email = visitor.Email,
                    City = visitor.City,
                    Postcode = visitor.Postcode,
                    EventId = eventItem.Id,
                    Visited = registration.Visited,
                    Confirmed = registration.Confirmed,
                    Title = date + " " + timeFrom + "-" + timeTill ,
                    NumberOfVisitors = registration.NumberOfVisitors
                });
            }

            ViewBag.TotalRegistrations = _registrationRepository.GetRegistrationTotal(start, end);
            ViewBag.TotalVisitors = _registrationRepository.GetVisitorTotal(start, end);

            return View(registrations.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(Guid id)
        {
            try
            {
                var item = _registrationRepository.GetById(id);
                var visitor = _visitorRepository.GetById(item.Visitor.Id);

                var scans = new List<ScanViewModel>();
                foreach (var scan in item.Scans)
                {
                    scans.Add(new ScanViewModel()
                    {
                        Id = scan.Id,
                        Timestamp = scan.Timestamp,
                        Status = scan.Status
                    });
                }

                var registrationViewModel = new RegistrationViewModel()
                {
                    Id = item.Id,
                    Name = visitor.Name,
                    Email = visitor.Email,
                    City = visitor.City,
                    Postcode = visitor.Postcode,
                    Visited = item.Visited,
                    Confirmed = item.Confirmed,
                    NumberOfVisitors = item.NumberOfVisitors,
                    Scans = scans
                };


                return View(registrationViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}