using System;
using Orchard.Environment.Extensions;
using Szmyd.Frooth.Services;

namespace Szmyd.Frooth.Providers {
    public class FroothZonesProvider : IZonesProvider {
        private readonly IZoneService _zoneService;

        public FroothZonesProvider (IZoneService zoneService) {
            _zoneService = zoneService;
        }

        #region Implementation of IZonesProvider

        public int Priority {
            get { return Int32.MaxValue - 1; }
        }

        /// <summary>
        /// Builds the zone hierarchy using the provided builder.
        /// </summary>
        /// <returns></returns>
        public void BuildZones(ZoneBuilder builder)
        {
            builder.ParentZone.Children = _zoneService.GetZones();
        }

        #endregion
    }
}