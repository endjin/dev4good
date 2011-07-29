namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;
    using System;
    using SharpArch.NHibernate.Web.Mvc;
    using System.Web.Routing;
    using System.Collections.Generic;
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

        public ActionResult ContactDetails()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("Application");
            }
            Applicant applicant = loginHelper.GetLoggedInApplicant();
            return View(applicant);
        }

        public ActionResult StartNewApplication()
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }

            Applicant loggedInAplicant = loginHelper.GetLoggedInApplicant();
            var result = commandProcessor.Process(new StartNewApplicationFormCommand(loggedInAplicant));
            var formResult = (ApplicationFormResult)result.Results[0];
            return new RedirectResult(string.Format("ContinueExistingApplication/{0}", formResult.ApplicationFormId));
        }

        public ActionResult ContinueExistingApplication(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult ApplicationForms(Applicant applicant)
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }
            
            SaveApplicantDetailsCommand saveDetailsCommand = new SaveApplicantDetailsCommand(applicant);
            commandProcessor.Process(saveDetailsCommand);
            Applicant currentApplicant = loginHelper.GetLoggedInApplicant();
            if (currentApplicant.Applications.Count == 0)
            {
                return new RedirectResult("StartNewApplication");
            }

            return View(currentApplicant);
        }

        public ActionResult ApplicationFormSection(int applicationFormId, int sectionId)
        {
            return View();
        }

    }
}