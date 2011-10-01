namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;

    #endregion

    public class HomeController : Controller
    {
        ILoginHelper loginHelper;

        public HomeController(ILoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        { 
            //TODO : Just mocking this up for now need to write a task to actually do the logging in
            loginHelper.LoginApplicant(email, password);
            return new RedirectResult("/Applicant/Edit");
        }
    }
}