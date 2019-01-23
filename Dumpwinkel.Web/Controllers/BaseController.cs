using Dumpwinkel.Logic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly SettingRepository _settingRepository = new SettingRepository();

        public BaseController()
        {
            var settings = _settingRepository.GetById(1);

            ViewBag.SiteTitle = settings.Title;
            ViewBag.TitleColor = settings.TitleColor;
            ViewBag.TitleSize = settings.TitleSize;
            ViewBag.IntroText = settings.IntroText;
            ViewBag.IntroTextColor = settings.IntroTextColor;
            ViewBag.IntroTextSize = settings.IntroTextSize;
            ViewBag.BackgroundImageUrl = settings.BackgroundImageUrl;
        }
    }
}