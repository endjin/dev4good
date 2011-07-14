namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    #endregion

    public class ApplicationController : Controller
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ILoginHelper loginHelper;
        private readonly IApplicantTasks applicantTasks;

        public ApplicationController(ICommandProcessor commandProcessor
                                     , ILoginHelper loginHelper
                                     , IApplicantTasks applicantTasks)
        {
            this.applicantTasks = applicantTasks;
            this.loginHelper = loginHelper;
            this.commandProcessor = commandProcessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("Application/ContactDetails");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Applicant applicant)
        {
            var command = new RegisterApplicantCommand(applicant);
            RegisterApplicantResult result = commandProcessor.Process(command) as RegisterApplicantResult;
            if (result == null)
            {
                return new RedirectResult("/");
            }
            else
            {
                loginHelper.LoginApplicant(result.ApplicantId);
            }

            return new RedirectResult("Application/ContactDetails");
        }

        public ActionResult ContactDetails()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("Application");
            }
            Applicant applicant = loginHelper.GetLoggedInApplicant();
            return View(applicant);
        }

        public ActionResult ApplicationFormSection(int sectionId)
        {
            return View();
        }

    }
}