using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CharityPortal.Data;

namespace CharityPortal.Controllers
{ 
    public class ResourceController : Controller
    {
        private DataContextContainer db = new DataContextContainer();

        //
        // GET: /Resource/

        public ViewResult Index()
        {
            return View(db.Resources.ToList());
        }

        //
        // GET: /Resource/Details/5

        public ViewResult Details(long id)
        {
            Resource resource = db.Resources.Single(r => r.Id == id);
            return View(resource);
        }

        //
        // GET: /Resource/Create

        public ActionResult Create()
        {
            ViewBag.Organizations = db.Organizations;
            ViewBag.Projects = db.Projects;
            return View();
        } 

        //
        // POST: /Resource/Create

        [HttpPost]
        public ActionResult Create(Resource resource)
        {

            foreach (var modelStateValue in ViewData.ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    // Do something useful with these properties
                    var errorMessage = error.ErrorMessage;
                    var exception = error.Exception;
                }
            }


            if (ModelState.IsValid)
            {
                int orgID = int.Parse(Request.Form["OrgCombo"].ToString());
                var orginization = db.Organizations.Where(X => X.Id == orgID).FirstOrDefault();

                resource.Organization = orginization;

                int projectID = int.Parse(Request.Form["ProjectCombo"].ToString());
                var project = db.Projects.Where(X => X.Id == projectID).FirstOrDefault();

                resource.Project = project;


                db.Resources.AddObject(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {

                ViewBag.Organizations = db.Organizations;
                ViewBag.Projects = db.Projects;
                return View(resource);
            }

           
        }
        
        //
        // GET: /Resource/Edit/5
 
        public ActionResult Edit(long id)
        {
            Resource resource = db.Resources.Single(r => r.Id == id);
             ViewBag.Organizations = db.Organizations;
            return View(resource);
        }

        //
        // POST: /Resource/Edit/5

        [HttpPost]
        public ActionResult Edit(Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Resources.Attach(resource);
                db.ObjectStateManager.ChangeObjectState(resource, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        //
        // GET: /Resource/Delete/5
 
        public ActionResult Delete(long id)
        {
            Resource resource = db.Resources.Single(r => r.Id == id);
            return View(resource);
        }

        //
        // POST: /Resource/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Resource resource = db.Resources.Single(r => r.Id == id);
            db.Resources.DeleteObject(resource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}