namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;
    #endregion

    public class ApplicationController : Controller
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly LoginHelper loginHelper;

        public ApplicationController(ICommandProcessor commandProcessor, LoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
            this.commandProcessor = commandProcessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("ContactDetails");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Applicant applicant)
        {
            var command = new RegisterApplicantCommand(applicant);
            RegisterApplicantResult result = commandProcessor.Process(command) as RegisterApplicantResult;
            if (result != null)
            {
                
            }

            return new RedirectResult("ContactDetails");
        }

        public ActionResult ContactDetails()
        {
            return View();
        }

        public ActionResult ApplicationFormSection(int sectionId)
        {
            return View();
        }

    }
}