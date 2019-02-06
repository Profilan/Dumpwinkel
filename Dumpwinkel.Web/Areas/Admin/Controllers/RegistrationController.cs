﻿using Dumpwinkel.Logic.Models;
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

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string startDate = null, string endDate = null, string eventId = null, string state = "all")
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

            if (String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                var dateNow = DateTime.Now.AddMonths(-1);
                start = DateTime.Now.AddMonths(-1).Date;
                end = DateTime.Now.AddMonths(2).Date;
            }
            else
            {
                start = Convert.ToDateTime(startDate);
                end = Convert.ToDateTime(endDate);
            }

            ViewBag.StartDate = start.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");
           


            var items = _registrationRepository.List(sortOrder, searchString, pageSize, pageNumber, start, end, eventId, state);

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

            return View(registrations.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(Guid id)
        {
            try
            {
                var item = _registrationRepository.GetById(id);
                var visitor = _visitorRepository.GetById(item.Visitor.Id);
                var registrationViewModel = new RegistrationViewModel()
                {
                    Id = item.Id,
                    Name = visitor.Name,
                    Email = visitor.Email,
                    City = visitor.City,
                    Postcode = visitor.Postcode,
                    Visited = item.Visited,
                    Confirmed = item.Confirmed,
                    NumberOfVisitors = item.NumberOfVisitors
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