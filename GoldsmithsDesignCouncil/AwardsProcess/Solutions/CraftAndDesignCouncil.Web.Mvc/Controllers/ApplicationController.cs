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
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels;
    #endregion

    public class ApplicationController : Controller
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ILoginHelper loginHelper;
        private readonly IApplicationFormSectionTasks applicationFormSectionTasks;
        private readonly IApplicationFormTasks applicationFormTasks;

        public ApplicationController(ICommandProcessor commandProcessor
                                        , IApplicationFormSectionTasks applicationFormSectionTasks
                                        , ILoginHelper loginHelper
                                        , IApplicationFormTasks applicationFormTasks)
        {
            this.applicationFormSectionTasks = applicationFormSectionTasks;
            this.loginHelper = loginHelper;
            this.commandProcessor = commandProcessor;
            this.applicationFormTasks = applicationFormTasks;
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
                return new RedirectResult("AplicationFormSection");
            }

            return View(currentApplicant);
        }

        public ActionResult ApplicationFormSection(int? applicationFormId, int? sectionId)
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }

            ApplicationForm form;
            ApplicationFormSection section;

            if (applicationFormId == null)
            {
                Applicant loggedInAplicant = loginHelper.GetLoggedInApplicant();
                form = applicationFormTasks.StartNewApplicationForm(loggedInAplicant);
            }
            else
            {
                form = applicationFormTasks.Get(applicationFormId.Value);
            }

            if (sectionId == null)
            {
                section = applicationFormSectionTasks.GetNextRequiredSectionForApplicationForm(form.Id);
            }
            else
            {
                section = applicationFormSectionTasks.Get(sectionId.Value);
            }

            var model = new ApplicationFormSectionViewModel();
            model.SectionTitle = string.Format("form {0} - Section {1}", form.Id, section.Id);
            return View(model);

        }
    }
}