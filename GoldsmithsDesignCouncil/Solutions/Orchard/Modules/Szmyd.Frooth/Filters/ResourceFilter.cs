using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Orchard.UI.Resources;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Services;
using ILogger = Orchard.Logging.ILogger;
using NullLogger = Orchard.Logging.NullLogger;
using ResourceLocation = Szmyd.Frooth.Models.ResourceLocation;


namespace Szmyd.Frooth.Filters {

    /// <summary>
    /// Filter for setting up layout shape.
    /// </summary>
    
    public class ResourceFilter : FilterProvider, IResultFilter {
        private readonly IContentManager _contentManager;
        private readonly IResourceService _service;
        private readonly IResourceManager _manager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRuleManager _ruleManager;

        public ResourceFilter(IContentManager contentManager, IResourceService service, IResourceManager manager, IWorkContextAccessor workContextAccessor, IRuleManager ruleManager) {
            _contentManager = contentManager;
            _service = service;
            _manager = manager;
            _workContextAccessor = workContextAccessor;
            _ruleManager = ruleManager;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; private set; }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // layers and widgets should only run on a full view rendering result
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            var workContext = _workContextAccessor.GetContext(filterContext);

            if (workContext == null ||
                workContext.Layout == null ||
                workContext.CurrentSite == null ||
                AdminFilter.IsApplied(filterContext.RequestContext)) {
                return;
            }

            IEnumerable<LayerPart> activeLayers = _contentManager.Query<LayerPart, LayerPartRecord>().List();

            foreach (var activeLayer in activeLayers) {
                // ignore the rule if it fails to execute
                try {
                    if (_ruleManager.Matches(activeLayer.Record.LayerRule)) {
                        var styles = _service.GetStylesForLayer(activeLayer.ContentItem.Id);
                        var scripts = _service.GetScriptsForLayer(activeLayer.ContentItem.Id);

                        foreach(var style in styles) {
                            _manager.Include("stylesheet", !string.IsNullOrWhiteSpace(style.Url) ? style.Url : style.LocalPath, null);
                        }

                        foreach(var script in scripts) {
                            var dependencies = new List<Resource>();
                            var current = script;
                            // Start from the highest dependency
                            while(current != null) {
                                dependencies.Add(current);
                                current = current.Dependency;
                            }
                            dependencies.Reverse();
                            foreach(var dep in dependencies)
                            {
                                var settings = _manager.Include("script", !string.IsNullOrWhiteSpace(dep.Url) ? dep.Url : dep.LocalPath, null);
                                if(dep.Location == ResourceLocation.Foot)
                                    settings.AtFoot();
                                else
                                    settings.AtHead();
                            }
                        }
                    }
                }
                catch(Exception e) {
                    Logger.Warning(e, T("An error occured during layer evaluation on: {0}", activeLayer.Name).Text);
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {}
    }
}