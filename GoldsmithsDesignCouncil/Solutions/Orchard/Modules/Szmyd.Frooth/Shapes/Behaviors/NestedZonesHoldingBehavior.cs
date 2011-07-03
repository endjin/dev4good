using System;
using System.Collections.Generic;
using System.Linq;
using ClaySharp;
using ClaySharp.Behaviors;
using Orchard;
using Orchard.DisplayManagement;
using Szmyd.Frooth.Services;

namespace Szmyd.Frooth.Shapes.Behaviors {
  
    /// <summary>
    /// Override for the default ZoneHoldingBehavior.
    /// Allows pre-building the zone collection before final addition 
    /// to the underlying shape (layout).
    /// Also allows to explicitly fire layout build, based on zones held and provided by
    /// ZoneManager.
    /// 
    /// * Returns a fake parent object for zones
    /// Foo.Zones 
    /// 
    /// * 
    /// Foo.Zones.Alpha : 
    /// Foo.Zones["Alpha"] 
    /// Foo.Alpha :same
    /// 
    /// </summary>
    public class FroothZonesBehavior : ClayBehavior, IDependency {
        private readonly Func<object> _zoneFactory;

        public FroothZonesBehavior(Func<object> zoneFactory) {
            _zoneFactory = zoneFactory;
            ZoneShapes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Zones held.
        /// </summary>
        private IDictionary<string, object> ZoneShapes { get; set; }

        /// <summary>
        /// Zone manager
        /// </summary>
        public Lazy<IZoneManager> ZoneManager { get; set; }

        /// <summary>
        /// Builds the final shape Items collection from underlying zones.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public object BuildZones(dynamic parent) {
            // Builds zone hierarchy, reusing any existing shapes
            var zoneObjects = ZoneManager.Value.GetZones();
            var zones = ZoneManager.Value == null
                                             ? ZoneShapes.Values
                                             : ZoneManager.Value.Shapify(_zoneFactory, zoneObjects, ZoneShapes);

            foreach (dynamic zone in zones.Where(z => !z.ShouldCollapse)) {
                zone.Parent = parent;
                var name = zone.ZoneName;
                parent.Add(zone);
            }

            return parent;
        }

        public override dynamic SetMemberMissing(Func<object> proceed, object self, string name, object value) {
            dynamic zone;
            dynamic potentialZone = value;
            var isShape = typeof (IShape).IsAssignableFrom(value.GetType());

            if (!isShape || potentialZone.Metadata.Type != "Zone") {
                return base.SetMember(proceed, self, name, value);
            }

            // If someone tries to explicitly add a zone (like layout.OnCreated event))
            if (!ZoneShapes.TryGetValue(name, out zone)) {
                ZoneShapes.Add(name, potentialZone);
            }
            else {
                ZoneShapes[name] = potentialZone;
            }

            return potentialZone;
        }

        public override object GetMember(Func<object> proceed, object self, string name) {
            if (name == "Zones") {
                // provide a robot for zone manipulation on parent object
                return ClayActivator.CreateInstance(new IClayBehavior[] {
                    new InterfaceProxyBehavior(),
                    new ZonesBehavior(_zoneFactory, self, ZoneShapes)
                });
            }

            if (name == "BuildZones") {
                return BuildZones(self);
            }

            dynamic foundZone;
            if (ZoneShapes.TryGetValue(name, out foundZone)) {
                return foundZone;
            }

            var result = proceed();

            if (((dynamic) result) == null) {
                // substitute nil results with a robot that turns adds a zone on
                // the parent when .Add is invoked
                return ClayActivator.CreateInstance(new IClayBehavior[] {
                    new InterfaceProxyBehavior(),
                    new NilBehavior(),
                    new ZoneOnDemandBehavior(_zoneFactory, self, name, ZoneShapes)
                });
            }
            return result;
        }

        public override object GetIndex(Func<object> proceed, IEnumerable<object> keys) {
            return keys.Count() == 1 ? GetMember(proceed, null, System.Convert.ToString(keys.Single())) : proceed();
        }

        #region Nested type: ZoneOnDemandBehavior

        public class ZoneOnDemandBehavior : ClayBehavior {
            private readonly object _parent;
            private readonly string _potentialZoneName;
            private readonly Func<object> _zoneFactory;
            private readonly IDictionary<string, object> _zoneShapes;

            public ZoneOnDemandBehavior(Func<object> zoneFactory, object parent, string potentialZoneName, IDictionary<string, object> zoneShapes) {
                _zoneFactory = zoneFactory;
                _parent = parent;
                _potentialZoneName = potentialZoneName;
                _zoneShapes = zoneShapes;
            }

            public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {
                int argsCount = args.Count();
                if (name == "Add" && (argsCount == 1 || argsCount == 2)) {
                    dynamic zone;
                    dynamic parent = _parent;
                    // Searching for an appropriate zone
                    // If found, add to the found zone. If not, add the zone
                    if (!_zoneShapes.TryGetValue(_potentialZoneName, out zone)) {
                        zone = parent[_potentialZoneName];
                        if (zone != null) {
                            _zoneShapes.Add(_potentialZoneName, zone);
                        }
                    }

                    if (zone == null) {
                        zone = _zoneFactory();
                        zone.Parent = parent;
                        zone.ZoneName = _potentialZoneName;
                        _zoneShapes.Add(_potentialZoneName, zone);
                    }

                    return argsCount == 1 ? zone.Add(args.Single()) : zone.Add(args.First(), (string) args.Last());
                }

                return proceed();
            }
        }

        #endregion

        #region Nested type: ZonesBehavior

        public class ZonesBehavior : ClayBehavior {
            private readonly object _parent;
            private readonly Func<object> _zoneFactory;
            private readonly IDictionary<string, object> _zoneShapes;

            public ZonesBehavior(Func<object> zoneFactory, object parent, IDictionary<string, object> zoneShapes) {
                _zoneFactory = zoneFactory;
                _parent = parent;
                _zoneShapes = zoneShapes;
            }

            public override object GetMember(Func<object> proceed, object self, string name) {
                dynamic parentMember;
                _zoneShapes.TryGetValue(name, out parentMember);
                parentMember = parentMember ?? ((dynamic) _parent)[name];

                if (parentMember == null) {
                    return ClayActivator.CreateInstance(new IClayBehavior[] {
                        new InterfaceProxyBehavior(),
                        new NilBehavior(),
                        new ZoneOnDemandBehavior(_zoneFactory, _parent, name, _zoneShapes)
                    });
                }
                return parentMember;
            }

            public override object GetIndex(Func<object> proceed, IEnumerable<object> keys) {
                return keys.Count() == 1 ? GetMember(proceed, null, System.Convert.ToString(keys.Single())) : proceed();
            }
        }

        #endregion
    }
}