namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using System.Web.Mvc;
    using CraftAndDesignCouncil.Infrastructure.Queries;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;

    public class ApplicationFormController : Controller
    {
        ILoginHelper loginHelper;
        IQueryRunner queryRunner;
        ICommandProcessor commandProcessor;

        public ApplicationFormController(IQueryRunner queryRunner, ILoginHelper loginHelper, ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
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

        public ActionResult Create()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }

            var cmd = new StartNewApplicationFormCommand();
            cmd.Applicant = loginHelper.GetLoggedInApplicant();
            var results = commandProcessor.Process(cmd);
            int newFormId = ((ApplicationFormResult)results.Results[0]).ApplicationFormId;
            return RedirectToRoute(new { Controller = "ApplicationFormSection", Action = "Edit", Id = newFormId });
        }
    }
}
