using Dumpwinkel.Web.Areas.Admin.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Dumpwinkel.Web.Areas.Admin.Controllers.Api
{
    public class UploadController : ApiController
    {
        [Route("api/upload/image")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage()
        {
            var root = HttpContext.Current.Server.MapPath("~/temp/uploads");
            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            var imagesFolder = System.Configuration.ConfigurationManager.AppSettings["ImagesFolder"];

            var originalFileName = GetDeserializedFileName(result.FileData.First());

            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
            string path = result.FileData.First().LocalFileName;

            var physicalDir = HttpContext.Current.Server.MapPath(imagesFolder);
            if (!Directory.Exists(physicalDir))
            {
                Directory.CreateDirectory(physicalDir);
            }
            var fileName = Path.GetFileNameWithoutExtension(originalFileName) + ".jpg";
            string physicalPath = physicalDir + @"/" + fileName;
            string virtualPath = imagesFolder + @"/" + fileName;
            if (FileHandler.Save(path, physicalPath))
            {
                File.Delete(path);

                return Request.CreateResponse(HttpStatusCode.OK, new { path = virtualPath, name = originalFileName }, JsonMediaTypeFormatter.DefaultMediaType);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private string GetDeserializedFileName(MultipartFileData multipartFileData)
        {
            var fileName = GetFileName(multipartFileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private string GetFileName(MultipartFileData multipartFileData)
        {
            return multipartFileData.Headers.ContentDisposition.FileName;
        }
    }
}
