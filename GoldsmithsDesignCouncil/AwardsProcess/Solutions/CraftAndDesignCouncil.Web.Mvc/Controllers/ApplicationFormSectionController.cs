using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    public class ApplicationFormSectionController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute(new { Controller = "ApplicationForm" });
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}
