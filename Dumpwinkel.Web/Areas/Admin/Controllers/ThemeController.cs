using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Dumpwinkel.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dumpwinkel.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "GRolDumpwinkelBeheerder,GRolDumpwinkelOrganisator")]
    public class ThemeController : Controller
    {
        private readonly ThemeRepository _themeRepository = new ThemeRepository();

        public ActionResult Index()
        {
            var items = _themeRepository.List();

            List<ThemeViewModel> themes = new List<ThemeViewModel>();
            foreach (Theme theme in items)
            {
                themes.Add(new ThemeViewModel()
                {
                    Id = theme.Id,
                    Title = theme.Title,
                    Description = theme.Description
                });
            }

            return View(themes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            try
            {
                Theme theme = Theme.Create(collection["Title"], collection["Description"]);

                _themeRepository.Insert(theme);

                Request.Flash("success", "Thema is opgeslagen");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Request.Flash("error", "Er is iets mis gegaan bij het opslaan");

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(Guid id)
        {
            try
            {
                Theme item = _themeRepository.GetById(id);

                if (item == null)
                {
                    Request.Flash("error", "Thema niet gevonden");

                    return RedirectToAction("Index");

                }

                ThemeViewModel model = new ThemeViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description
                };

                return View(model);
            }
            catch (Exception e)
            {
                Request.Flash("error", "Er is iets mis gegaan: " + e.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                Theme item = _themeRepository.GetById(new Guid(collection["Id"]));

                item.Title = collection["Title"];
                item.Description = collection["Description"];

                _themeRepository.Update(item);

                Request.Flash("success", "Thema is gewijzigd");

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                Request.Flash("error", "Er is iets mis gegaan: " + e.Message);

                return RedirectToAction("Index");
            }
        }
    }
}