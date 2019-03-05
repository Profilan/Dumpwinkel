using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly VisitorRepository _visitorRepository = new VisitorRepository();
        private readonly ThemeRepository _themeRepository = new ThemeRepository();

        [AllowAnonymous]
        public ActionResult Create(Guid id)
        {
            try
            {
                var eventItem = _eventRepository.GetById(id);

                var themeDescription = "";
                var themeTitle = "";
                if (eventItem.Theme != null)
                {
                    Theme theme = _themeRepository.GetById(eventItem.Theme.Id);
                    if (theme != null)
                    {
                        themeTitle = theme.Title.ToUpper();
                        themeDescription = theme.Description;
                    }
                }

                var registrationViewModel = new RegistrationViewModel()
                {
                    Title = eventItem.TimeRange.ToString(),
                    EventId = eventItem.Id,
                    ThemeTitle = themeTitle,
                    ThemeDescription = themeDescription
                };

                return View(registrationViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var eventItem = _eventRepository.GetById(new Guid(collection["EventId"]));

                var visitor = _visitorRepository.GetByEmail(collection["Email"]);
                if (visitor == null)
                {
                    visitor = Visitor.Create(collection["Name"], collection["City"], collection["Email"], collection["Postcode"]);
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

                var registration = Registration.Create(visitor, eventItem, numberOfVisitors, false);

                _registrationRepository.Insert(registration);

                string themeTitle = "";
                if (eventItem.Theme != null)
                {
                    Theme theme = _themeRepository.GetById(eventItem.Theme.Id);
                    themeTitle = "[" + theme.Title + "]"; 
                }

                var activationUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/registration/confirm/" + registration.Id;
                var logoUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/img";
                ActivationEmail email = new ActivationEmail()
                {
                    To = collection["Email"],
                    ActivationUrl = activationUrl,
                    Name = collection["Name"],
                    Date = eventItem.TimeRange.Start.ToString("dd-MM-yyyy"),
                    TimeFrom = eventItem.TimeRange.Start.ToShortTimeString(),
                    TimeTill = eventItem.TimeRange.End.ToShortTimeString(),
                    NumberOfVisitors = numberOfVisitors,
                    LogoUrl = logoUrl,
                    ThemeTitle = themeTitle,
                };
                email.Send();

                return RedirectToAction("ThankYou");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Confirm(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                registration.Confirmed = true;
                _registrationRepository.Update(registration);

                var eventItem = _eventRepository.GetById(registration.Event.Id);
                //eventItem.UpdateMaximumNumberOfVisitors(eventItem.MaximumNumberOfVisitors - registration.NumberOfVisitors);
                //_eventRepository.Update(eventItem);

                var visitor = _visitorRepository.GetById(registration.Visitor.Id);

                string themeTitle = "";
                if (eventItem.Theme != null)
                {
                    Theme theme = _themeRepository.GetById(eventItem.Theme.Id);
                    themeTitle = "[" + theme.Title + "]";
                }

                var logoUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/img";
                //var barcodeUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/fonts/Code39.woff";
                //var barcodeUrl = @"https://www.barcodesinc.com/generator/image.php?code=" + registration.Id.ToString().ToUpper() + "&style=197&type=C39&width=590&height=100&xres=1&font=4";
                var barcodeUrl = @"https://chart.googleapis.com/chart?chl=" + Uri.EscapeUriString(registration.Id.ToString()) + @"&chs=200x200&cht=qr&chld=H%7C0";

                var fileName = eventItem.Id + ".pdf";
                var temp = Path.GetTempPath();
                var path = Path.Combine(temp, fileName);

                ConfirmationEmail email = new ConfirmationEmail()
                {
                    To = visitor.Email,
                    Name = visitor.Name,
                    Date = eventItem.TimeRange.Start.ToString("dd-MM-yyyy"),
                    TimeFrom = eventItem.TimeRange.Start.ToShortTimeString(),
                    TimeTill = eventItem.TimeRange.End.ToShortTimeString(),
                    NumberOfVisitors = registration.NumberOfVisitors,
                    LogoUrl = logoUrl,
                    BarcodeUrl = barcodeUrl,
                    RegistrationId = registration.Id.ToString(),
                    Disclaimer = _settings.EmailDisclaimer,
                    ThemeTitle = themeTitle
                };
                //email.GeneratePDF(path, visitor.Name, eventItem.TimeRange, registration.NumberOfVisitors);
                //email.Attach(new Attachment(path));
                email.Send();

                return RedirectToAction("Confirmed");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult ThankYou()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AlreadyRegistered()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Confirmed()
        {
            return View();
        }
    }
}