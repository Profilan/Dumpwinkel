using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {

        private readonly SettingRepository _settingRepository = new SettingRepository();

        public ActionResult Edit(int id)
        {
            var settings = _settingRepository.GetById(1);

            var imagesFolder = System.Configuration.ConfigurationManager.AppSettings["ImagesFolder"];
            var physicalDir = System.Web.HttpContext.Current.Server.MapPath(imagesFolder);
            var images = Directory.GetFiles(physicalDir);
            var imageUrls = new List<string>();
            foreach (var image in images)
            {
                imageUrls.Add(imagesFolder + @"/" + Path.GetFileName(image));
            }

            var viewModel = new SettingViewModel()
            {
                Id = settings.Id,
                Title = settings.Title,
                TitleColor = settings.TitleColor,
                TitleSize = settings.TitleSize,
                IntroText = settings.IntroText,
                IntroTextColor = settings.IntroTextColor,
                IntroTextSize = settings.IntroTextSize,
                BackgroundImageUrl = settings.BackgroundImageUrl,
                MaxFileSize = 512,
                AcceptFileTypes = @"/(\.|\/)(jpg)$/i",
                UploadUrl = "/api/upload/image",
                Images = imageUrls
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                var settings = _settingRepository.GetById(1);

                settings.Title = collection["Title"];
                settings.TitleColor = collection["TitleColor"];
                settings.TitleSize = Convert.ToInt32(collection["TitleSize"]);
                settings.IntroText = collection["IntroText"];
                settings.IntroTextColor = collection["IntrotextColor"];
                settings.IntroTextSize = Convert.ToInt32(collection["IntroTextSize"]);
                settings.BackgroundImageUrl = collection["ImageUrl"];

                _settingRepository.Update(settings);

                return RedirectToAction("Edit", new { id = 1 });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}