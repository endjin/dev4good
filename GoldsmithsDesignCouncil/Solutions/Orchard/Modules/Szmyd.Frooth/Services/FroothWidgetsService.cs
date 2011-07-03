using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Widgets.Services;

namespace Szmyd.Frooth.Services
{
    public class FroothWidgetsService : WidgetsService, IWidgetsService
    {
        private readonly IZoneManager _zoneManager;

        public FroothWidgetsService(IZoneManager zoneManager, IContentManager contentManager, IFeatureManager featureManager)
            : base(contentManager, featureManager) {
            _zoneManager = zoneManager;
        }

        public new IEnumerable<string> GetZones()
        {
            return _zoneManager.GetNames().Distinct();
        }
    }
}