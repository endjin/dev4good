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
        IApplicantTasks applicantTasks;
        ICommandProcessor commandProcessor;
        ILoginHelper loginHelper;

        public ApplicantController(IApplicantTasks applicantTasks, ICommandProcessor commandProcessor, ILoginHelper loginHelper)
        {
            this.loginHelper = loginHelper;
            this.applicantTasks = applicantTasks;
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
            try
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
            catch
            {
                return View();
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
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
              
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
