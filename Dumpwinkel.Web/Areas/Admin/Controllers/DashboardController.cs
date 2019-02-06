using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBeheerder")]
    public class DashboardController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}