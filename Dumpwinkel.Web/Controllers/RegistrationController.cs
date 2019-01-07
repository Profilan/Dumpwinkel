using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Logic.Services;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly VisitorRepository _visitorRepository = new VisitorRepository();

        public ActionResult Create(Guid id)
        {
            try
            {
                var eventItem = _eventRepository.GetById(id);

                var registrationViewModel = new RegistrationViewModel()
                {
                    Title = eventItem.TimeRange.ToString(),
                    EventId = eventItem.Id,

                };

                return View(registrationViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var eventItem = _eventRepository.GetById(new Guid(collection["EventId"]));

                var visitor = _visitorRepository.GetByEmail(collection["Email"]);
                if (visitor == null)
                {
                    visitor = Visitor.Create(collection["Name"], collection["City"], collection["Email"]);
                    _visitorRepository.Insert(visitor);
                }

                // Check if the visitor already registered on this day
                var registrations = _registrationRepository.GetByVisitorAndEvent(visitor, eventItem);
                if (registrations.Count() > 0)
                {
                    if (registrations.Last().Confirmed == true)
                    {
                        return RedirectToAction("AlreadyRegistered");
                    }
                }

                var numberOfVisitors = Convert.ToInt32(collection["NumberOfVisitors"]);

                var registration = Registration.Create(visitor, eventItem, numberOfVisitors);

                _registrationRepository.Insert(registration);

                var mailService = new EmailService();
                var message = "Beste " + collection["Name"] + ",<br /><br />";
                message += "Bedankt voor je aanvraag. Door op de volgende link te klikken kun je de registratie bevestigen.<br /><br />";
                message += Request.Url.GetLeftPart(UriPartial.Authority) + "/registration/confirm/" + registration.Id + "<br /><br />";
                message += "Met vriendelijke groet,<br />";
                message += "De Eekhoorn Dumpwinkel";
                mailService.SendMail("beheerder@deeekhoorn.com", collection["Email"], "Dumpwinkel registratie aanvraag", message);

                return RedirectToAction("ThankYou");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public ActionResult Confirm(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                registration.Confirmed = true;
                _registrationRepository.Update(registration);

                var eventItem = _eventRepository.GetById(registration.Event.Id);
                eventItem.UpdateMaximumNumberOfVisitors(eventItem.MaximumNumberOfVisitors - registration.NumberOfVisitors);
                _eventRepository.Update(eventItem);

                var visitor = _visitorRepository.GetById(registration.Visitor.Id);

                var fileName = eventItem.Id + ".pdf";
                var temp = Path.GetTempPath();
                var path = Path.Combine(temp, fileName);

                var mailService = new EmailService();
                var message = "Beste " + visitor.Name + ",<br /><br />";
                message += "Uw aanvraag is gelukt. Bijgevoegd vindt u de registratie. Deze moet u meenemen bij uw bezoek aan de dumpwinkel.<br /><br />";
                message += "Met vriendelijke groet,<br />";
                message += "De Eekhoorn Dumpwinkel";

                mailService.GeneratePDF(path, visitor.Name, eventItem.TimeRange, registration.NumberOfVisitors);
                mailService.SendMail("beheerder@deeekhoorn.com", visitor.Email, "Dumpwinkel registratie bevestiging", message, path);

                return RedirectToAction("Confirm");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult AlreadyRegistered()
        {
            return View();
        }

        public ActionResult Confirm()
        {
            return View();
        }
    }
}