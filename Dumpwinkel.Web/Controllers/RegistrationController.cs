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
        private readonly SettingRepository _settingRepository = new SettingRepository();

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

                var ipAddress = GetIPAddress();

                var visitor = _visitorRepository.GetByEmail(collection["Email"]);
                if (visitor == null)
                {
                    visitor = Visitor.Create(collection["Name"], collection["City"], collection["Email"], collection["Postcode"]);
                    
                    _visitorRepository.Insert(visitor);
                }

                /*
                // Check if the visitor already registered on this day or earlier than legacy deadline
                var registrations = _registrationRepository.GetByVisitorAndEvent(visitor.Id, eventItem);
                if (registrations.Count() > 0)
                {
                    DateTime legacyDate = eventItem.TimeRange.Start;
                    switch (_settings.LegacyPeriod.Unit)
                    {
                        case Profilan.SharedKernel.Unit.Hours:
                            legacyDate = eventItem.TimeRange.Start.AddHours(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        case Profilan.SharedKernel.Unit.Minutes:
                            legacyDate = eventItem.TimeRange.Start.AddMinutes(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        case Profilan.SharedKernel.Unit.Seconds:
                            legacyDate = eventItem.TimeRange.Start.AddSeconds(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        case Profilan.SharedKernel.Unit.Days:
                            legacyDate = eventItem.TimeRange.Start.AddDays(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        case Profilan.SharedKernel.Unit.Months:
                            legacyDate = eventItem.TimeRange.Start.AddMonths(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        case Profilan.SharedKernel.Unit.Years:
                            legacyDate = eventItem.TimeRange.Start.AddYears(-1 * _settings.LegacyPeriod.Amount);
                            break;
                        default:
                            break;
                    }

                    // Haal de bezochte registraties op (bezocht of bevestigd?)
                    var visitedRegistrations = _registrationRepository.GetVisitedByVisitor(visitor.Id);

                    if (visitedRegistrations.Count() > 0)
                    {
                        var lastEvent = _eventRepository.GetById(visitedRegistrations.Last().Event.Id);
                        if (lastEvent.TimeRange.Start >= legacyDate)
                        {
                            return RedirectToAction("EarlyRegistered");
                        }
                    }


                    if (registrations.Last().Confirmed == true)
                    {
                        return RedirectToAction("AlreadyRegistered");
                    }
                }
                */

                // Voeg registratie toe
                var numberOfVisitors = Convert.ToInt32(collection["NumberOfVisitors"]);
                var registration = Registration.Create(visitor, eventItem, numberOfVisitors, false);
                registration.IPAddress = ipAddress;
                _registrationRepository.Insert(registration);


                // Verstuur een activatie email
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
                var eventItem = _eventRepository.GetById(registration.Event.Id);
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);

                var ipAddress = GetIPAddress();

                // Controleer of in dit event registraties zijn vanaf hetzelfde ip adres
                bool alreadyRegistered = false;
                var otherRegistrations = _registrationRepository.ListByEventAndIp(eventItem.Id, ipAddress);
                if (otherRegistrations.Count() > 0) // Er zijn registraties vanaf hetzelfe IP
                {
                    foreach (var otherRegistration in otherRegistrations)
                    {
                        if (otherRegistration.Confirmed)
                        {
                            registration.RejectionReason = "Al geregistreerd (IP)";
                            _registrationRepository.Update(registration);

                            // Dit was dezelfde bezoeker, dus pas dit aan
                            visitor = otherRegistration.Visitor;

                            alreadyRegistered = true;
                            break;
                        }
                    }
                }

                if (!alreadyRegistered)
                {
                    // Controleer of in dit event registraties zijn vanaf hetzelde email adres
                    otherRegistrations = _registrationRepository.GetByVisitorAndEvent(visitor.Id, eventItem);
                    if (otherRegistrations.Count() > 0) // Er zijn registraties vanaf hetzelfe E-mail adres
                    {
                        foreach (var otherRegistration in otherRegistrations)
                        {
                            if (otherRegistration.Confirmed)
                            {
                                registration.RejectionReason = "Al geregistreerd (Email)";
                                _registrationRepository.Update(registration);

                                // Dit was dezelfde bezoeker, dus pas dit aan
                                visitor = otherRegistration.Visitor;

                                alreadyRegistered = true;
                                break;
                            }
                        }
                    }
                }

                // Controleer of de registratie buiten het termijn valt
                bool earlierRegistered = false;
                if (!alreadyRegistered)
                {
                    var registrations = _registrationRepository.GetByVisitorAndNotEvent(visitor.Id, eventItem);
                    if (registrations.Count() > 0)
                    {
                        DateTime legacyDate = eventItem.TimeRange.Start;
                        switch (_settings.LegacyPeriod.Unit)
                        {
                            case Profilan.SharedKernel.Unit.Hours:
                                legacyDate = eventItem.TimeRange.Start.AddHours(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            case Profilan.SharedKernel.Unit.Minutes:
                                legacyDate = eventItem.TimeRange.Start.AddMinutes(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            case Profilan.SharedKernel.Unit.Seconds:
                                legacyDate = eventItem.TimeRange.Start.AddSeconds(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            case Profilan.SharedKernel.Unit.Days:
                                legacyDate = eventItem.TimeRange.Start.AddDays(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            case Profilan.SharedKernel.Unit.Months:
                                legacyDate = eventItem.TimeRange.Start.AddMonths(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            case Profilan.SharedKernel.Unit.Years:
                                legacyDate = eventItem.TimeRange.Start.AddYears(-1 * _settings.LegacyPeriod.Amount);
                                break;
                            default:
                                break;
                        }

                        // Haal de bezochte registraties op (bezocht of bevestigd?)
                        var visitedRegistrations = _registrationRepository.GetVisitedByVisitor(visitor.Id);
                        

                        if (visitedRegistrations.Count() > 0)
                        {
                            var lastVisitedRegistration = visitedRegistrations.Last();
                            var lastEvent = _eventRepository.GetById(lastVisitedRegistration.Event.Id);
                            if (lastEvent.TimeRange.Start >= legacyDate)
                            {
                                // Update de registratie met de reden van afwijzing
                                lastVisitedRegistration.RejectionReason = "Eerder geregistreerd (E-mail)";
                                _registrationRepository.Update(lastVisitedRegistration);

                                earlierRegistered = true;
                            }
                        }
                    }

                }

                if (earlierRegistered)
                {
                    return RedirectToAction("EarlyRegistered");
                }

                if (alreadyRegistered)
                {
                    return RedirectToAction("AlreadyRegistered");
                }

                registration.Confirmed = true;
                registration.ConfirmationDate = DateTime.Now;
                _registrationRepository.Update(registration);

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
        public ActionResult EarlyRegistered()
        {
            ViewBag.LegacyText = _settings.LegacyText;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Confirmed()
        {
            return View();
        }


        private string GetIPAddress()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

    }
}