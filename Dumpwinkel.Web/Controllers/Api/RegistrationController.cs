using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models.Api;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Route("api/registration/checkin/{id}")]
        public IHttpActionResult Checkin(Guid id)
        {
            try
            {
                var registration = _registrationRepository.GetById(id);
                var visitor = _visitorRepository.GetById(registration.Visitor.Id);
                var eventItem = _eventRepository.GetById(registration.Event.Id);

                var currentDate = DateTime.Now;

                if (registration.Visited)
                {
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
    }
}
