namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;

    #endregion

    public class ApplicationController : Controller
    {
        public ActionResult Index()
        {
            ActionResult res = View();
            return res;
        }
    }
}