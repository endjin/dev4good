using System.Collections.Generic;
using System.Linq;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.ViewModels {
    public class ZoneManagementViewModel  {
        public ZoneManagementViewModel() {
            Zones = Enumerable.Empty<Zone>().ToList();
        }

        public IEnumerable<Zone> AvailableZones { get; set; }

        public Zone NewZone { get; set; }

        public IList<Zone> Zones { get; set; }
    }
}
