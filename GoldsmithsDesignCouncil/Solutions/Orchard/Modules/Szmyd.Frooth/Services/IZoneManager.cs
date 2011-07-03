using System;
using System.Collections.Generic;
using Orchard;
using Orchard.Environment.Extensions;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Providers;

namespace Szmyd.Frooth.Services
{
    public interface IZoneManager : IDependency {
        IEnumerable<IZonesProvider> Providers { get; }
        IEnumerable<Zone> GetBasicZones();
        IEnumerable<Zone> GetBasicZonesLinear();
        IEnumerable<Zone> GetZones();
        IEnumerable<Zone> GetZonesLinear();
        IEnumerable<string> GetNames();
        IEnumerable<dynamic> Shapify(Func<dynamic> shapeFactory, IEnumerable<Zone> zones = null, IDictionary<string, dynamic> existingShapes = null);
    }
}