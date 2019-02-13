using Dumpwinkel.Logic.Models;
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
        protected Setting _settings;

        public BaseController()
        {
            _settings = _settingRepository.GetById(1);

            ViewBag.SiteTitle = _settings.Title;
            ViewBag.TitleColor = _settings.TitleColor;
            ViewBag.TitleSize = _settings.TitleSize;
            ViewBag.IntroText = _settings.IntroText;
            ViewBag.IntroTextColor = _settings.IntroTextColor;
            ViewBag.IntroTextSize = _settings.IntroTextSize;
            ViewBag.InfoText = _settings.InfoText;
            ViewBag.BackgroundImageUrl = _settings.BackgroundImageUrl;
        }
    }
}