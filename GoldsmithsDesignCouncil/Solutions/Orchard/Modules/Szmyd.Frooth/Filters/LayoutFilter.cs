using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClaySharp;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Szmyd.Frooth.Services;
using Szmyd.Frooth.Shapes.Behaviors;
using ILogger = Orchard.Logging.ILogger;
using NullLogger = Orchard.Logging.NullLogger;

namespace Szmyd.Frooth.Filters {

    /// <summary>
    /// Filter for setting up layout shape.
    /// </summary>
    
    public class LayoutFilter : FilterProvider, IResultFilter {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly Lazy<IZoneManager> _zoneManager;


        public LayoutFilter(Lazy<IZoneManager> zoneManager, IWorkContextAccessor workContextAccessor) {
            _zoneManager = zoneManager;
            _workContextAccessor = workContextAccessor;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; private set; }

        #region IResultFilter Members

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // layers and widgets should only run on a full view rendering result
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null) {
                return;
            }

            WorkContext workContext = _workContextAccessor.GetContext(filterContext);

            if (workContext == null ||
                workContext.Layout == null ||
                workContext.CurrentSite == null ||
                AdminFilter.IsApplied(filterContext.RequestContext)) {
                // Dynamic layout doesn't apply to the admin view (at least for now:)
                return;
            }

            FroothZonesBehavior zonesBehavior = ((IEnumerable<IClayBehavior>) workContext.Layout.Behavior).OfType<FroothZonesBehavior>().Single();
            zonesBehavior.ZoneManager = _zoneManager;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {}

        #endregion
    }
}