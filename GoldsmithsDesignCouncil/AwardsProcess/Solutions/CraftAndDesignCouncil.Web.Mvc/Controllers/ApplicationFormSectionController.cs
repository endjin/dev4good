using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    public class ApplicationFormSectionController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute(new { Controller = "ApplicationForm" });
        }

        //id 1 is the form and id2 is the section
        public ActionResult Edit(int id, int? id2)
        {
            var model = new ApplicationFormSectionViewModel();
            model.SectionTitle = "Test Section Title";
            return View(model);
        }
    }
}
