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
    public class ProjectController : Controller
    {
        private DataContextContainer db = new DataContextContainer();

        //
        // GET: /Project/

        public ViewResult Index()
        {
            return View(db.Projects.ToList());
        }

        //
        // GET: /Project/Details/5

        public ViewResult Details(int id)
        {
            Project project = db.Projects.Single(p => p.Id == id);
            return View(project);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            ViewBag.Organizations = db.Organizations;
            return View();
        } 

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                int orgID = int.Parse(Request.Form["AdminOrg"].ToString());
                var orginization = db.Organizations.Where(X => X.Id == orgID).FirstOrDefault();
                
                project.AdminOrganization = orginization;
                
                db.Projects.AddObject(project);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(project);
        }
        
        //
        // GET: /Project/Edit/5
 
        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Single(p => p.Id == id);
            ViewBag.Organizations = db.Organizations;
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Attach(project);
                db.ObjectStateManager.ChangeObjectState(project, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //
        // GET: /Project/Delete/5
 
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Single(p => p.Id == id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Project project = db.Projects.Single(p => p.Id == id);
            db.Projects.DeleteObject(project);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                //report error
                return View();
            } 
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}