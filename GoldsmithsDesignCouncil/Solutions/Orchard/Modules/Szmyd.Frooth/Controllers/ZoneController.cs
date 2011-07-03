using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.AntiForgery;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Providers;
using Szmyd.Frooth.Services;
using Szmyd.Frooth.ViewModels;

namespace Szmyd.Frooth.Controllers {
    /// <summary>
    /// Admin controller for operation on zones.
    /// </summary>
    [ValidateInput(false), Admin]
    public class ZoneController : Controller, IUpdateModel {
        private readonly IOrchardServices _services;
        private readonly ITransactionManager _transactionManager;
        private readonly IZoneManager _zoneManager;
        private readonly IZoneService _zoneService;

        public ZoneController(
            IZoneService zoneService,
            IZoneManager zoneManager,
            IOrchardServices services,
            ITransactionManager transactionManager) {
            _zoneService = zoneService;
            _zoneManager = zoneManager;
            _services = services;
            _transactionManager = transactionManager;
        }

        public Localizer T { get; set; }

        #region IUpdateModel Members

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        #endregion

        public ActionResult Index(ZoneManagementViewModel model) {
            if (!_services.Authorizer.Authorize(Permissions.ManageZones, T("Not allowed to manage site layout"))) {
                return new HttpUnauthorizedResult();
            }

            if (model == null) {
                model = new ZoneManagementViewModel();
            }

            // Creating zones to choose parent from
            model.AvailableZones = new[] {
                new Zone {
                    Id = 0,
                    Position = 0,
                    Name = T("- No parent -").Text,
                    ParentName = ""
                }
            };
            model.AvailableZones = model.AvailableZones.Concat(_zoneManager.GetBasicZonesLinear());

            if (model.Zones == null || model.Zones.Count() < 1) {
                model.Zones = _zoneManager.GetBasicZones().ToList();
            }

            // need action name as this action is referenced from another action
            return View("Index", model);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST(IList<Zone> zones) {
            if (!_services.Authorizer.Authorize(Permissions.ManageZones, T("Couldn't manage the site layout"))) {
                return new HttpUnauthorizedResult();
            }

            if (zones != null) {
                try {
                    foreach (Zone entry in zones.Where(z => z != null && !z.IsReadOnly)) {
                        _zoneService.UpdateZone(entry);
                    }
                }
                catch (Exception ex) {
                    _services.Notifier.Add(NotifyType.Error, T("Error when updating zones: {0}", ex.Message));
                    _transactionManager.Cancel();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateZone() {
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(ZoneManagementViewModel model) {
            if (!_services.Authorizer.Authorize(Permissions.ManageZones, T("Couldn't manage the site layout"))) {
                return new HttpUnauthorizedResult();
            }

            if (!ModelState.IsValid) {
                return View("Index", model);
            }

            try {
                _zoneService.CreateZone(model.NewZone.Name, model.NewZone.ParentName, model.NewZone.IsCollapsible, model.NewZone.IsVertical, model.NewZone.Tag);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Error when creating zone: {0}", ex.Message));
                _transactionManager.Cancel();
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Delete(int id) {
            if (!_services.Authorizer.Authorize(Permissions.ManageZones, T("Couldn't manage the site layout"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                _zoneService.DeleteZone(id);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Error when deleting zone: {0}", ex.Message));
                _transactionManager.Cancel();
            }


            return RedirectToAction("Index");
        }

        public ActionResult IndexLayer(int id) {
            return null;
        }

        public ActionResult IndexLayerPOST() {
            return null;
        }

        public ActionResult MoveZoneUp(int id) {
            return null;
        }

        public ActionResult MoveZoneDown(int id) {
            return null;
        }
    }
}