namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;
    using System;
    using SharpArch.NHibernate.Web.Mvc;
    #endregion

    public class ApplicationController : Controller
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ILoginHelper loginHelper;

        public ApplicationController(ICommandProcessor commandProcessor, ILoginHelper loginHelper)
        {
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
            ApplicantResult result = commandProcessor.Process(command) as ApplicantResult;
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

        [HttpGet]
        public ActionResult ContactDetails()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("Application");
            }
            Applicant applicant = loginHelper.GetLoggedInApplicant();
            return View(applicant);
        }

        [HttpPost]
        public ActionResult ContactDetails(Applicant applicant)
        {
            SaveApplicantDetailsCommand saveDetailsCommand = new SaveApplicantDetailsCommand(applicant);
            ApplicantResult result = commandProcessor.Process(saveDetailsCommand) as ApplicantResult;
            return new RedirectResult("Application/ApplicationFormSection");
        }

        public ActionResult ApplicationFormSection()
        {
            return View();
        }

        public ActionResult ApplicationFormSection(int sectionId)
        {
            return View();
        }

    }
}