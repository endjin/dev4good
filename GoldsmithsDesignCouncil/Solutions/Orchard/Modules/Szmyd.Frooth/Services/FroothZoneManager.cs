using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Notify;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Providers;

namespace Szmyd.Frooth.Services
{
    public class FroothZoneManager : IZoneManager
    {
        private readonly INotifier _notifier;
        private readonly ICacheManager _cacheManager;

        public FroothZoneManager(INotifier notifier, IEnumerable<IZonesProvider> providers, ICacheManager cacheManager)
        {
            _notifier = notifier;
            Providers = providers;
            _cacheManager = cacheManager;
        }

        public Localizer T { get; set; }

        #region Implementation of IZoneManager

        public IEnumerable<IZonesProvider> Providers { 
            get;
            protected set;
        }

        public IEnumerable<Zone> GetBasicZones() {
            // Returns all zones without the alternate overrides for layers
            return Hierarchize(Merge(Flatten(Providers.Where(p => p.GetType() != typeof (FroothAlternateZonesProvider)).SelectMany(BuildProviderZones).ToList()))).ToList();
        }

        public IEnumerable<Zone> GetBasicZonesLinear()
        {
            // Returns all zones without the alternate overrides for layers
            return Flatten(GetBasicZones());
        }

        public IEnumerable<Zone> GetZones() {
            return _cacheManager.Get("Frooth.Zones", ctx => 
                Hierarchize(Merge(Flatten(Providers.SelectMany(BuildProviderZones).ToList()))).ToList()
            );
        }

        private IEnumerable<Zone> BuildProviderZones(IZonesProvider provider) {

            // Needs to take care of forgotten Priority settings.
            var priority = provider.Priority != default(int) ? provider.Priority : Providers.Max(p => p.Priority) + 10;
            
            var builder = new ZoneBuilder(priority);

            try
            {
                provider.BuildZones(builder);
            }
            catch (Exception ex) {
                _notifier.Error(T(ex.Message));
                return Enumerable.Empty<Zone>();
            }

            return builder.ParentZone.Children;
        }

        public IEnumerable<Zone> GetZonesLinear()
        {
            return Flatten(GetZones());
        }

        public IEnumerable<string> GetNames()
        {
            return GetZonesLinear().Select(z => z.Name);
        }

        /// <summary>
        /// Creates shapes from given zone list. Reuses exisiting zone shapes, if necessary.
        /// </summary>
        /// <param name="shapeFactory"></param>
        /// <param name="zones"></param>
        /// <param name="existingShapes"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Shapify(Func<dynamic> shapeFactory, IEnumerable<Zone> zones = null, IDictionary<string, dynamic> existingShapes = null) {
            zones = zones ?? GetZones();

            foreach (var zone in zones) {
                dynamic shape;

                if (existingShapes == null || !existingShapes.TryGetValue(zone.Name, out shape)) {
                    shape = shapeFactory();
                }

                shape.ZoneName = zone.Name;
                shape.Current = zone;

                if (zone.Children.Count() > 0) {
                    var children = Shapify(shapeFactory, zone.Children, existingShapes).Where(c => !c.ShouldCollapse);
                    foreach (var childShape in children)
                    {
                        childShape.Parent = shape;
                        shape.Add(childShape, childShape.Current.Position.ToString());
                    }
                }

                // Collapsed if there are no children
                shape.ShouldCollapse = ((IEnumerable<dynamic>)shape).Count() == 0 && zone.IsCollapsible;
                yield return shape;
            }
        }

        #endregion

        #region Private static methods

        private static IEnumerable<Zone> Flatten(IEnumerable<Zone> source)
        {
            return source != null ? source.Concat(source.SelectMany(z => Flatten(z.Children))) : Enumerable.Empty<Zone>();
        }

        private static IEnumerable<Zone> Merge(IEnumerable<Zone> source)
        {
            var retval = source
                .GroupBy(
                    z => z.Name,
                    e => e,
                    (key, list) => list.OrderByDescending(z => z.Priority).First()).Where(z => !z.IsRemoved);
            return retval;
        }

        private static IEnumerable<Zone> Hierarchize(IEnumerable<Zone> source)
        {
            // Repair the hierarchy
            // We have to rejoin children to parents after merging zones from different sources
            foreach (var zone in source) {
                zone.Children = source.Where(z => z.ParentName == zone.Name).ToList();
            }

            return source.Where(z => string.IsNullOrWhiteSpace(z.ParentName));
        }
        #endregion

    }
}