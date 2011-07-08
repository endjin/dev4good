

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginApplicant(string email, string password)
        {
            //TODO : Just mocking this up for now need to write a task to actually do the logging in
            Session["LOGGED_IN_USER"] = 1;
            return new RedirectResult("/");
        }

        public ActionResult LogoutApplicant()
        {
            Session.Remove("LOGGED_IN_USER");
            return new RedirectResult("/");
        }
    }
}
