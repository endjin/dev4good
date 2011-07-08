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

        public ApplicationController(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Applicant applicant)
        {
            var command = new RegisterApplicantCommand(applicant);
            RegisterApplicantResult result = commandProcessor.Process(command) as RegisterApplicantResult;
            if (result != null)
            {
                Session["LOGGED_IN_USER"] = result.ApplicantId;
            }

            return new RedirectResult("ApplicationForm");
        }
    }
}