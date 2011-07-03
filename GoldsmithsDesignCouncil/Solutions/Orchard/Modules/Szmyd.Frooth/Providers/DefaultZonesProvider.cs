using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Environment.Features;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.Providers {
    public class DefaultZonesProvider : IZonesProvider {
        private readonly IFeatureManager _featureManager;

        public DefaultZonesProvider (IFeatureManager featureManager) {
            _featureManager = featureManager;
        }

        /// <summary>
        /// Returns the tree structure of zone definitions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Zone> GetZones() {
            int i = 0;
            var retval = _featureManager.GetEnabledFeatures()
                .Select(x => x.Extension)
                .Where(x => DefaultExtensionTypes.IsTheme(x.ExtensionType))
                .SelectMany(x => x.Zones.Split(','))
                .Concat(new[] { "Content" })
                .Distinct()
                .Select(x => new Zone
                {
                    Name = x.Trim(),
                    IsVertical = false,
                    IsCollapsible = true,
                    Position = i++,
                    Tag = "div",
                    Priority = Priority
                }).ToList();
            return retval;
        }

        #region Implementation of IZonesProvider

        public int Priority {
            get { return Int32.MinValue; }
        }

        /// <summary>
        /// Builds the zone hierarchy using the provided builder.
        /// </summary>
        /// <returns></returns>
        public void BuildZones(ZoneBuilder builder) {
            builder.ParentZone.Children = GetZones();
        }

        #endregion
    }
}