using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBalieMedewerker, GRolDumpwinkelBeheerder")]
    public class CheckinController : BaseController
    {
        private readonly RegistrationRepository _registrationRepository = new RegistrationRepository();
        
        public ActionResult Index()
        {
            return View();
        }
    }
}