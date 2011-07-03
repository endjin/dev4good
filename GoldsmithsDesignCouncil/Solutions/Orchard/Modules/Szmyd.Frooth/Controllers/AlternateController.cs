using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.AntiForgery;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Providers;
using Szmyd.Frooth.Services;
using Szmyd.Frooth.ViewModels;

namespace Szmyd.Frooth.Controllers
{
    /// <summary>
    /// Admin controller for operations on zone alternates
    /// </summary>
    [ValidateInput(false), Admin]
    public class AlternateController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _services;
        private readonly ITransactionManager _transactionManager;
        private readonly IWidgetsService _widgetsService;
        private readonly IZoneManager _zoneManager;
        private readonly IZoneService _zoneService;

        public AlternateController(
            IZoneService zoneService,
            IZoneManager zoneManager,
            IOrchardServices services,
            ITransactionManager transactionManager, IWidgetsService widgetsService)
        {
            _zoneService = zoneService;
            _zoneManager = zoneManager;
            _services = services;
            _transactionManager = transactionManager;
            _widgetsService = widgetsService;
        }

        public Localizer T { get; set; }

        #region IUpdateModel Members

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Not allowed to manage alternate layouts.")))
            {
                return new HttpUnauthorizedResult();
            }

            LayerPart firstLayer = _widgetsService.GetLayers().FirstOrDefault();
            if (firstLayer == null)
            {
                return View("Index");
            }

            return RedirectToAction("Layer", new { layerId = firstLayer.ContentItem.Id });
        }

        [HttpGet]
        public ActionResult Layer(int layerId, AlternateManagementViewModel model)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Not allowed to manage alternate layouts.")))
            {
                return new HttpUnauthorizedResult();
            }

            LayerPart layer = _widgetsService.GetLayer(layerId);
            if (layer == null)
            {
                return new HttpNotFoundResult();
            }

            if (model == null)
                model = new AlternateManagementViewModel();

            model.AvailableZones = _zoneManager.GetZonesLinear();
            model.Layers = _widgetsService.GetLayers();
            model.LayerId = layer.ContentItem.Id;
            model.Layer = layer;
            model.AvailableParents =
                new[] {
                    new ZoneAlternate {
                        Id = default(int),
                        ZoneName = "- No parent -"
                    }
                }.Concat(_zoneService.GetAlternatesForLayerFlat(layer.ContentItem.Id));
            model.Alternates = _zoneService.GetAlternatesForLayer(layer.ContentItem.Id).ToList();

            // Getting preview zones
            foreach (FroothAlternateZonesProvider altProvider in _zoneManager.Providers.OfType<FroothAlternateZonesProvider>())
            {
                altProvider.LayerId = layer.ContentItem.Id;
            }

            model.PreviewZones = _zoneManager.GetZones();

            return View("Layer", model);
        }

        [HttpPost, ActionName("Layer")]
        public ActionResult LayerPOST(int layerId, IList<ZoneAlternate> zones)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Couldn't manage alternate layouts")))
            {
                return new HttpUnauthorizedResult();
            }

            if (zones != null)
            {
                try
                {
                    foreach (ZoneAlternate entry in zones.Where(z => z != null))
                    {
                        _zoneService.UpdateAlternate(entry);
                    }
                }
                catch (Exception ex)
                {
                    _services.Notifier.Add(NotifyType.Error, T("Error when updating alternates: {0}", ex.Message));
                    _transactionManager.Cancel();
                }
            }

            return RedirectToAction("Layer", layerId);
        }

        public ActionResult Create(int layerId)
        {
            return RedirectToAction("Layer", layerId);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(AlternateManagementViewModel model)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Couldn't manage the alternate layouts.")))
            {
                return new HttpUnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return View("Layer", model);
            }

            try
            {
                _zoneService.CreateAlternate(model.New.ZoneName, model.LayerId, model.New.Parent.Id, model.New.IsCollapsible, model.New.IsVertical, model.New.IsRemoved, model.New.Tag);
            }
            catch (Exception ex)
            {
                _services.Notifier.Add(NotifyType.Error, T("Error when creating alternate: {0}", ex.Message));
                _transactionManager.Cancel();
            }

            return RedirectToAction("Layer", new { model.LayerId, model });
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Delete(int id)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Couldn't manage the alternate layouts.")))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                _zoneService.DeleteAlternate(id);
            }
            catch (Exception ex)
            {
                _services.Notifier.Add(NotifyType.Error, T("Error when deleting alternate: {0}", ex.Message));
                _transactionManager.Cancel();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Clear(int layerId)
        {
            if (!_services.Authorizer.Authorize(Permissions.ManageAlternates, T("Couldn't manage the alternate layouts.")))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                _zoneService.DeleteAlternatesForLayer(layerId);
            }
            catch (Exception ex)
            {
                _services.Notifier.Add(NotifyType.Error, T("Error when clearing layer: {0}", ex.Message));
                _transactionManager.Cancel();
            }

            return RedirectToAction("Layer", layerId);
        }

        public ActionResult MoveUp(int id)
        {
            ZoneAlternate alt = _zoneService.GetZoneAlternate(id);
            _zoneService.MoveUp(alt);
            return RedirectToAction("Layer", alt.LayerId);
        }

        public ActionResult MoveDown(int id)
        {
            ZoneAlternate alt = _zoneService.GetZoneAlternate(id);
            _zoneService.MoveDown(alt);
            return RedirectToAction("Layer", alt.LayerId);
        }
    }
}