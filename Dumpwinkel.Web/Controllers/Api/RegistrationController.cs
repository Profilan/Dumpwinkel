using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using Dumpwinkel.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Dumpwinkel.Web.Controllers.Api
{
    public class RegistrationController : ApiController
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        private readonly VisitorRepository _visitorRepository = new VisitorRepository();
        private readonly EventRepository _eventRepository = new EventRepository();
        private readonly SettingRepository _settingRepository = new SettingRepository();

        [HttpPost]
        [Route("api/registration/checkin/{id}")]
        public IHttpActionResult Checkin(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                if (registration == null)
                {
                    var message = new CheckinMessage()
                    {
                        Status = 602,
                        Description = "Onbekende ticket"
                    };

                    var response = new CheckinResponse()
                    {
                        Data = null,
                        Message = message
                    };
                    return Content(HttpStatusCode.BadRequest, response);
                }

                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);

                var currentDate = DateTime.Now;


                if (registration.Visited)
                {
                    var scan = Scan.Create(DateTime.Now, "Ticket is al gebruikt", registration);
                    registration.Scans.Add(scan);
                    _registrationRepository.Update(registration);

                    var body = new CheckinBody()
                    {
                        Name = visitor.Name,
                        City = visitor.City,
                        Email = visitor.Email,
                        NumberOfVisitors = registration.NumberOfVisitors,
                        Postcode = visitor.Postcode,
                        TimeSlot = eventItem.TimeRange.ToString()
                    };

                    var message = new CheckinMessage()
                    {
                        Status = 601,
                        Description = "Ticket is al gebruikt"
                    };

                    var response = new CheckinResponse()
                    {
                        Data = body,
                        Message = message
                    };
                    return Content(HttpStatusCode.BadRequest, response);
                }

                if (currentDate >= eventItem.TimeRange.Start && currentDate <= eventItem.TimeRange.End)
                {
                    var scan = Scan.Create(DateTime.Now, "Geaccepteerd", registration);
                    registration.Scans.Add(scan);

                    var body = new CheckinBody()
                    {
                        Name = visitor.Name,
                        City = visitor.City,
                        Email = visitor.Email,
                        NumberOfVisitors = registration.NumberOfVisitors,
                        Postcode = visitor.Postcode,
                        TimeSlot = eventItem.TimeRange.ToString()
                    };

                    var message = new CheckinMessage()
                    {
                        Status = 200,
                        Description = "Geaccepteerd"
                    };

                    var response = new CheckinResponse()
                    {
                        Data = body,
                        Message = message
                    };

                    registration.Visited = true;
                    _registrationRepository.Update(registration);

                    return Ok(response);
                }
                else
                {
                    var scan = Scan.Create(DateTime.Now, "Ticket valt buiten de toegestane timeslot", registration);
                    registration.Scans.Add(scan);
                    _registrationRepository.Update(registration);

                    var body = new CheckinBody()
                    {
                        Name = visitor.Name,
                        City = visitor.City,
                        Email = visitor.Email,
                        NumberOfVisitors = registration.NumberOfVisitors,
                        Postcode = visitor.Postcode,
                        TimeSlot = eventItem.TimeRange.ToString()
                    };

                    var message = new CheckinMessage()
                    {
                        Status = 600,
                        Description = "Ticket valt buiten de toegestane timeslot"
                    };

                    var response = new CheckinResponse()
                    {
                        Data = body,
                        Message = message
                    };
                    return Content(HttpStatusCode.BadRequest, response);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        [Route("api/registration/accept/{id}")]
        public IHttpActionResult Accept(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);

                var scan = Scan.Create(DateTime.Now, "Alsnog geaccepteerd", registration);
                registration.Scans.Add(scan);

                var body = new CheckinBody()
                {
                    Name = visitor.Name,
                    City = visitor.City,
                    Email = visitor.Email,
                    NumberOfVisitors = registration.NumberOfVisitors,
                    Postcode = visitor.Postcode,
                    TimeSlot = eventItem.TimeRange.ToString()
                };

                var message = new CheckinMessage()
                {
                    Status = 200,
                    Description = "Alsnog geaccepteerd"
                };

                var response = new CheckinResponse()
                {
                    Data = body,
                    Message = message
                };

                registration.Visited = true;
                _registrationRepository.Update(registration);

                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/registration/deny/{id}")]
        public IHttpActionResult Deny(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);

                var scan = Scan.Create(DateTime.Now, "Geweigerd", registration);
                registration.Scans.Add(scan);

                var body = new CheckinBody()
                {
                    Name = visitor.Name,
                    City = visitor.City,
                    Email = visitor.Email,
                    NumberOfVisitors = registration.NumberOfVisitors,
                    Postcode = visitor.Postcode,
                    TimeSlot = eventItem.TimeRange.ToString()
                };

                var message = new CheckinMessage()
                {
                    Status = 605,
                    Description = "Geweigerd"
                };

                var response = new CheckinResponse()
                {
                    Data = body,
                    Message = message
                };

                _registrationRepository.Update(registration);

                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("api/registration/create")]
        public IHttpActionResult Create([FromBody]RegistrationViewModel data)
        {
            try
            {
                var eventItem = _eventRepository.GetById(data.EventId);

                var visitor = _visitorRepository.GetByEmail(data.Email);
                if (visitor == null)
                {
                    visitor = Visitor.Create(data.Name, data.City, data.Email, data.Postcode);
                    _visitorRepository.Insert(visitor); 
                }

                var numberOfVisitors = Convert.ToInt32(data.NumberOfVisitors);

                var registration = Registration.Create(visitor, eventItem, numberOfVisitors, false);
                registration.Confirmed = true;
                registration.ConfirmationDate = DateTime.Now;

                _registrationRepository.Insert(registration);

                var logoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/img";
                var barcodeUrl = @"https://chart.googleapis.com/chart?chl=" + Uri.EscapeUriString(registration.Id.ToString()) + @"&chs=200x200&cht=qr&chld=H%7C0";

                var settings = _settingRepository.GetById(1);

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
                    Disclaimer = settings.EmailDisclaimer,
                };
                
                email.Send();

                return Ok(new { message = "Registratie is verzonden en bevestigd" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("api/Registration/Delete/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var notification = _registrationRepository.GetById(id);

                _registrationRepository.Delete(id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Content(HttpStatusCode.NoContent, "Registratie is met succes verwijderd.");
        }
    }
}
