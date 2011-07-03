using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Media.Services;
using Orchard.Mvc.AntiForgery;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Services;
using Szmyd.Frooth.ViewModels;

namespace Szmyd.Frooth.Controllers {
    /// <summary>
    /// Admin controller for styling and scripting operations (download/upload CSS and scripts).
    /// </summary>
    [ValidateInput(false), Admin]
    public class ResourceController : Controller, IUpdateModel {
        private readonly IResourceService _resources;
        private readonly IOrchardServices _services;
        private readonly IMediaService _media;
        private readonly IWidgetsService _widgets;

        public ResourceController(IMediaService media, IWidgetsService widgets, IResourceService resources, IOrchardServices services) {
            _media = media;
            _widgets = widgets;
            _resources = resources;
            _services = services;
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

        public ActionResult Index() {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            var model = new ResourcesViewModel {
                AvailableScripts = _resources.GetScripts(),
                AvailableStyles = _resources.GetStyles(),
                LayerId = default(int),
                Layers = _widgets.GetLayers()
            };

            return View("Index", model);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST(IList<Resource> model) {
            return RedirectToAction("Index");
        }

        public ActionResult Create() {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryTokenOrchard]
        public ActionResult CreatePOST(ResourcesViewModel model) {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                Resource newResource = model.New;

                if (model.New == null)
                {
                    _services.Notifier.Add(NotifyType.Error, T("Form values provided were empty."));
                    return RedirectToAction("Index");
                }

                if (String.IsNullOrWhiteSpace(Request.Files[0].FileName) && string.IsNullOrWhiteSpace(newResource.Url))
                {
                    _services.Notifier.Add(NotifyType.Error, T("Provide an URL or select a file to upload"));
                    return RedirectToAction("Index");
                }

                if (newResource.Url == null) {
                    foreach (string fileName in Request.Files) {
                        newResource.LocalPath = _media.UploadMediaFile("Frooth/", Request.Files[fileName], false);
                        newResource.Url = null;
                    }
                }
                else {
                    newResource.LocalPath = null;
                }
                
                _resources.CreateResource(newResource);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Cannot create resource: {0}", ex.Message));
            }

            return RedirectToAction("Index");
        }

        public ActionResult Layer(int layerId) {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            LayerPart layer = _widgets.GetLayer(layerId);
            if (layer == null) {
                _services.Notifier.Add(NotifyType.Error, T("Layer {0} doesn't exist.", layerId));
                return RedirectToAction("Index");
            }
            var layerScripts = _resources.GetScriptsForLayer(layerId);
            var layerStyles = _resources.GetStylesForLayer(layerId);

            var model = new ResourcesViewModel {
                AvailableScripts = _resources.GetScripts().Where(s => !layerScripts.Any(ex => ex.Id == s.Id)),
                AvailableStyles = _resources.GetStyles().Where(s => !layerStyles.Any(ex => ex.Id == s.Id)),
                ScriptsInLayer = layerScripts,
                StylesInLayer = layerStyles,
                LayerId = layerId,
                Layers = _widgets.GetLayers()
            };

            return View("Layer", model);
        }

        [ValidateAntiForgeryTokenOrchard]
        public ActionResult Delete(int id) {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                _resources.DeleteResource(id);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Cannot delete resource: {0}", ex.Message));
            }

            return RedirectToAction("Index");
        }

        public ActionResult LayerAdd(int resourceId, int layerId) {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            LayerPart layer = _widgets.GetLayer(layerId);
            Resource resource = _resources.GetResource(resourceId);
            if (layer == null || resource == null) {
                _services.Notifier.Add(NotifyType.Error, T("Error! Specified layer or resource doesn't exist."));
                return RedirectToAction("Index");
            }

            try {
                _resources.AddToLayer(resourceId, layerId);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Cannot add resource {0} to layer {1}: {2}", resourceId, layerId, ex.Message));
            }

            return RedirectToAction("Layer", new{ layerId });
        }

        public ActionResult LayerRemove(int resourceId, int layerId) {
            if (!_services.Authorizer.Authorize(Permissions.ManageResources, T("Couldn't manage the layout resources"))) {
                return new HttpUnauthorizedResult();
            }

            LayerPart layer = _widgets.GetLayer(layerId);
            Resource resource = _resources.GetResource(resourceId);

            if (layer == null || resource == null) {
                _services.Notifier.Add(NotifyType.Error, T("Error! Specified layer or resource doesn't exist."));
                return RedirectToAction("Index");
            }

            try {
                _resources.RemoveFromLayer(resourceId, layerId);
            }
            catch (Exception ex) {
                _services.Notifier.Add(NotifyType.Error, T("Cannot remove resource {0} from layer {1}: {2}", resourceId, layerId, ex.Message));
            }

            return RedirectToAction("Layer", new { layerId });
        }
    }
}