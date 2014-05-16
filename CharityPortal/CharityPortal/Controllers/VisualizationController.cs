using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CharityPortal.Data;
using Newtonsoft.Json;
using CharityPortal.Models;
using CharityPortal.Utils;

namespace CharityPortal.Controllers
{
    public class VisualizationController : Controller
    {
        public ActionResult OverviewMap()
        {
            using (DataContextContainer dataContext = new DataContextContainer()) {
                IList<Resource> availableResources = dataContext.Organizations.SelectMany(org => org.AvailableResources).ToList();
                IList<Project> projects = dataContext.Projects.ToList();
                IList<Resource> requiredResources = projects.SelectMany(project => project.RequiredResources).ToList();
                


                OverviewMapViewModel model = new OverviewMapViewModel();

                foreach (Resource availableResource in availableResources)
                    model.Map.Markers.Add(MapUtils.CreateAvailableResourceMarker(availableResource, Url));

                foreach (Resource requiredResource in requiredResources)
                    model.Map.Markers.Add(MapUtils.CreateRequiredResourceMarker(requiredResource, Url));

                foreach (Project project in projects)
                    model.Map.Markers.Add(MapUtils.CreateProjectMarker(project, Url));

                return View(model);
            }
        }
    }
}
