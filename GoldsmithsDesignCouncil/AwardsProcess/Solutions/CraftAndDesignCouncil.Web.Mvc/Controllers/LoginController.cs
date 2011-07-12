

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    public class LoginController : Controller
    {
        private readonly LoginHelper loginHelper;

        public LoginController(LoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginApplicant(string email, string password)
        {
            //TODO : Just mocking this up for now need to write a task to actually do the logging in
            loginHelper.LoginApplicant(email, password);
            return new RedirectResult("/");
        }

        public ActionResult LogoutApplicant()
        {
            loginHelper.LogoutApplicant();
            return new RedirectResult("/");
        }
    }
}
