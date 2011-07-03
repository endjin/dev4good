using System.Collections.Generic;
using System.Linq;
using Orchard.Widgets.Models;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.ViewModels {
    public class AlternateManagementViewModel  {
        public AlternateManagementViewModel() {
            Alternates = Enumerable.Empty<ZoneAlternate>().ToList();
        }

        /// <summary>
        /// All layers
        /// </summary>
        public IEnumerable<LayerPart> Layers { get; set; }

        /// <summary>
        /// Selected layer Id
        /// </summary>
        public int LayerId { get; set; }

        public LayerPart Layer { get; set; }

        /// <summary>
        /// Zones available for creating alternate for
        /// </summary>
        public IEnumerable<Zone> AvailableZones { get; set; }

        /// <summary>
        /// Newly created alternate data
        /// </summary>
        public ZoneAlternate New { get; set; }

        /// <summary>
        /// Existing alternates for layer
        /// </summary>
        public IList<ZoneAlternate> Alternates { get; set; }

        /// <summary>
        /// List of zone names already altered in this layer.
        /// </summary>
        public IEnumerable<ZoneAlternate> AvailableParents { get; set; }

        /// <summary>
        /// Merged zones for preview of the final rendering.
        /// </summary>
        public IEnumerable<Zone> PreviewZones { get; set; }
    }
}
