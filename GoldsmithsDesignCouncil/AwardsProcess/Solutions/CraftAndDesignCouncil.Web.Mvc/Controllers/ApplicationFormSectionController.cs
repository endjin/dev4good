using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives
    using System.Web.Mvc;
    using CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using CraftAndDesignCouncil.Infrastructure.Queries;
    #endregion

    public class ApplicationFormSectionController : Controller
    {
        IApplicationFormSectionTasks applicationFormSectionTasks;
        IQueryRunner queryRunner;
        ILoginHelper loginHelper;

        public ApplicationFormSectionController(IApplicationFormSectionTasks applicationFormSectionTasks,
                                                  ILoginHelper loginHelper,
                                                   IQueryRunner queryRunner )
        {
            this.applicationFormSectionTasks = applicationFormSectionTasks;
            this.loginHelper = loginHelper;
        }

        public ActionResult Index()
        {
            return RedirectToRoute(new { Controller = "ApplicationForm" });
        }

        //id is the form and id2 is the section
        public ActionResult Edit(int id, int? id2)
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return Redirect("/");
            }
            var user = loginHelper.GetLoggedInApplicant();
            var form = user.GetApplicationById(id);
            if (form == null)
            {
                return RedirectToRoute(new { Controller = "ApplicationForm" });
            }

            ApplicationFormSection section;
            if (id2 == null)
            {
                section = applicationFormSectionTasks.GetNextRequiredSectionForApplicationForm(form.Id);
            }
            else
            {
                var query = new GetSectionByIdQuery {sectionId=id2.Value};
                section = queryRunner.RunQuery(query).First();
            }
            
            var model = new ApplicationFormSectionViewModel();
            var questions = new List<QuestionAndAnswerViewModel>();
            foreach (Question question in section.Questions)
            {
               questions.Add(new QuestionAndAnswerViewModel{QuestionText = question.QuestionText});
            }
            model.Questions = questions;
            model.SectionTitle = section.Title;
            return View(model);
        }
    }
}