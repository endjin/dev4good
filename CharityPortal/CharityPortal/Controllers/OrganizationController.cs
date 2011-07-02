using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CharityPortal.Data;
using System.Configuration;
using System.Data.Entity;

namespace CharityPortal.Controllers
{
    public class OrganizationController : Controller
    {
        //
        // GET: /Organisation/

        public ActionResult Index()
        {
            var context = new DataContextContainer( ConfigurationManager.ConnectionStrings["DataContextContainer"].ConnectionString);
            List<Organization> organizations = context.Organizations.ToList();
            return View(organizations);
        }

        //
        // GET: /Organisation/Details/5

        public ActionResult Details(int id)
        {
            var context = new DataContextContainer();
                        Organization organization = context.Organizations.Where(X => X.Id == id).FirstOrDefault();
            return View(organization);
        }

        //
        // GET: /Organisation/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Organisation/Create

        [HttpPost]
        public ActionResult Create(Organization model)
        {
            try
            {
                var context = new DataContextContainer();
                context.AddToOrganizations(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Organisation/Edit/5
 
        public ActionResult Edit(int id)
        {
            var context = new DataContextContainer();
            Organization organization = context.Organizations.Where(X => X.Id == id).FirstOrDefault();
            return View(organization);
        }

        //
        // POST: /Organisation/Edit/5

        [HttpPost]
        public ActionResult Edit(Organization model)
        {
            try
            {
                // TODO: Add update logic here
                var context = new DataContextContainer();
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Organisation/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Organisation/Delete/5

        [HttpPost]
        public ActionResult Delete(Organization model)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
