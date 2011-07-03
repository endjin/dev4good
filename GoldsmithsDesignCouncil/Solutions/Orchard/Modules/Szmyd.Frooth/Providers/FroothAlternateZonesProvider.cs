using System;
using Orchard.Environment.Extensions;
using Szmyd.Frooth.Services;

namespace Szmyd.Frooth.Providers {
    /// <summary>
    /// Provider of zone alternates based on the currently viewed layer(s).
    /// </summary>
    public class FroothAlternateZonesProvider : IZonesProvider {
        private readonly IZoneService _zoneService;

        public FroothAlternateZonesProvider(IZoneService zoneService) {
            _zoneService = zoneService;
        }

        #region Implementation of IZonesProvider

        internal int LayerId
        {
            get;
            set;
        }

        /// <summary>
        /// Zone overriding order (higher overrides lower).
        /// </summary>
        /// <remarks>
        /// Use values >5, as the lower numbers are reserved for default providers. 
        /// Otherwise you can get unpredictable results.
        /// </remarks>
        public int Priority {
            get { return Int32.MaxValue; }
        }

        /// <summary>
        /// Builds the zone hierarchy using the provided builder.
        /// </summary>
        /// <returns></returns>
        public void BuildZones(ZoneBuilder builder) {
            // Small hack to be able to generate preview for a given layer
            // without the need to explicitely create manager object
            builder.ParentZone.Children = (LayerId == default(int)) 
                ? _zoneService.GetCurrentAlternateZones()
                : _zoneService.GetAlternateZonesForLayer(LayerId);
        }

        #endregion
    }
}