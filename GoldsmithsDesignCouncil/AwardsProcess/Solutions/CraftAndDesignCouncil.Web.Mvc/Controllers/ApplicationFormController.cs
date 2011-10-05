namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using System.Web.Mvc;
    using CraftAndDesignCouncil.Infrastructure.Queries;

    public class ApplicationFormController : Controller
    {
        ILoginHelper loginHelper;
        IQueryRunner queryRunner;

        public ApplicationFormController(IQueryRunner queryRunner, ILoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
            this.queryRunner = queryRunner;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }

            var applicant = loginHelper.GetLoggedInApplicant();
            var query = new ListApplicationFormsForApplicantQuery {Applicant= applicant};
            var forms = queryRunner.RunQuery(query);
            return View(forms);
        }
    }
}
