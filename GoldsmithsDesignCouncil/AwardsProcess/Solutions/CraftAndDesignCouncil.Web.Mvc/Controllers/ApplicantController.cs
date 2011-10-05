using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftAndDesignCouncil.Domain;
using CraftAndDesignCouncil.Domain.Contracts.Tasks;
using CraftAndDesignCouncil.Tasks.Commands;
using SharpArch.Domain.Commands;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    public class ApplicantController : Controller
    {
        ICommandProcessor commandProcessor;
        ILoginHelper loginHelper;

        public ApplicantController(ICommandProcessor commandProcessor, ILoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
            this.commandProcessor = commandProcessor;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Edit");
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Applicant applicant)
        {
            var command = new RegisterApplicantCommand(applicant); 
            var tmpResult = commandProcessor.Process(command).Results[0];
            ApplicantResult result = tmpResult as ApplicantResult;
            if (result == null)
            {
                return View();
            }
            else
            {
                loginHelper.LoginApplicant(result.ApplicantId);
                return RedirectToAction("Edit", new { id = result.ApplicantId });
            }
        }
        
 
        public ActionResult Edit()
        {
            if (loginHelper.SomebodyIsLoggedIn)
            {
                return View(loginHelper.GetLoggedInApplicant());
            }
            else
            {
                return RedirectToRoute("default", null);
            }
            
        }

        [HttpPost]
        public ActionResult Edit(Applicant applicant)
        {
            if (!loginHelper.SomebodyIsLoggedIn)
            {
                return new RedirectResult("/");
            }

            SaveApplicantDetailsCommand saveDetailsCommand = new SaveApplicantDetailsCommand(applicant);
            commandProcessor.Process(saveDetailsCommand);

            return new RedirectResult("/ApplicationForm/List");
        }
    }
}
