using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CharityPortal.Data;
using Newtonsoft.Json;
using CharityPortal.Models;

namespace CharityPortal.Controllers
{
    public class VisualizationController : Controller
    {
        public ActionResult OverviewMap()
        {
            OverviewMapViewModel model = new OverviewMapViewModel();

            model.Map = new Map() {
                Markers = new List<Marker>() {
                    new Marker() {
                        Title = "Playground 1",
                        Content = "Work in progress",
                        Latitude = -34.397,
                        Longitude = 150.644
                    }
                }
            };

            return View(model);
        }

    }
}
