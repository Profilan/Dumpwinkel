using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Dumpwinkel.Web.Areas.Admin.Controllers.Api
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/image/delete/{url}")]
        public IHttpActionResult Delete(string url)
        {
            try
            {
                var file = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(url));
                var physicalFile = HttpContext.Current.Server.MapPath(file);
                File.Delete(physicalFile);

                return Ok(file);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
