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