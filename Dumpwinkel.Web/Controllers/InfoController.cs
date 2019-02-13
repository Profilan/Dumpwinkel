using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers
{
    public class InfoController : BaseController
    {
        // GET: Info
        public ActionResult Index()
        {
            return View();
        }
    }
}