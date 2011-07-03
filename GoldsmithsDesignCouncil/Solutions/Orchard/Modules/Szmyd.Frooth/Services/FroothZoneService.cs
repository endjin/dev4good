using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Utilities;
using Zone = Szmyd.Frooth.Models.Zone;

namespace Szmyd.Frooth.Services
{
    /// <summary>
    /// Default implementation of zone service for cached CRUD operations.
    /// </summary>
    public class FroothZoneService : IZoneService
    {
        #region Variables
        
        public const string SignalName = "Frooth.Zones.Changed";
        private readonly ICacheManager _cache;
        private readonly IRepository<ZoneRecord> _repository;
        private readonly IRepository<ZoneAlternateRecord> _altRepository;
        private readonly ISignals _signals;
        private readonly IRuleManager _ruleManager;
        private readonly IContentManager _contentManager;

        #endregion

        #region Constructor
        
        public FroothZoneService(
            IRepository<ZoneRecord> repository, IRepository<ZoneAlternateRecord> altRepository,
            ICacheManager cache, ISignals signals, IRuleManager ruleManager, IContentManager contentManager)
        {
            _repository = repository;
            _altRepository = altRepository;
            _cache = cache;
            _signals = signals;
            _ruleManager = ruleManager;
            _contentManager = contentManager;
        }

        #endregion

        #region Implementation of IZoneService

        #region Zone operations
        /// <summary>
        /// Retrieves hierarchized (parent/children) collection of existing zones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Zone> GetZones()
        {
            return DefaultZones();
        }

        /// <summary>
        /// Gets specified zone.
        /// </summary>
        /// <param name="zoneId">Id of the zone.</param>
        /// <returns></returns>
        public Zone GetZone(int zoneId)
        {
            var part = MapToModel(_repository.Get(zoneId));
            return part;
        }

        /// <summary>
        /// Gets specified zone by name.
        /// </summary>
        /// <param name="zoneName">Name of the zone.</param>
        /// <returns></returns>
        public Zone GetZone(string zoneName)
        {
            return MapToModel(_repository.Get(z => z.Name == zoneName));
        }

        /// <summary>
        /// Deletes specified zone
        /// </summary>
        /// <param name="zoneId"></param>
        public void DeleteZone(int zoneId)
        {
            var zone = _repository.Get(zoneId);
            var children = _repository.Fetch(z => z.ParentName == zone.Name);

            foreach (var child in children) {
                child.Parent = null;
                child.ParentName = null;
            }

            DeleteAlternatesForZone(zone.Name);
            _repository.Delete(zone);
            _repository.Flush();
            TriggerSignal();
        }

        /// <summary>
        /// Creates new zone.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <param name="isCollapsible"></param>
        /// <param name="isVertical"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Zone CreateZone(string name, string parentName, bool isCollapsible, bool isVertical, string tag)
        {
            var parent = !string.IsNullOrWhiteSpace(parentName) ? _repository.Get(z => z.Name == parentName) : null;
            var siblings = _repository.Fetch(z => z.Parent == parent || z.ParentName == parentName);
            var record = new ZoneRecord
            {
                Name = name,
                Parent = parent,
                Tag = tag,
                ParentName = !string.IsNullOrWhiteSpace(parentName) ? parentName : null,
                Position = siblings.Count() > 0 ? siblings.Max(z => z.Position) + 1 : 1,
                IsCollapsible = isCollapsible,
                IsVertical = isVertical
            };
            _repository.Create(record);
            _repository.Flush();
            TriggerSignal();
            return MapToModel(record);
        }

        /// <summary>
        /// Updates zone from model.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public Zone UpdateZone(Zone zone)
        {
            var oldRecord = _repository.Get(zone.Id);
            var newRecord = MapFromModel(zone);

            if (oldRecord.Name != newRecord.Name)
            {
                // Have to change the parent name for all child records
                foreach (var child in _repository.Fetch(z => z.ParentName == oldRecord.Name))
                {
                    child.ParentName = newRecord.Name;
                }

                // Have to change the alternate relation name for all child records
                foreach (var child in _altRepository.Fetch(z => z.ZoneName == oldRecord.Name))
                {
                    child.ZoneName = newRecord.Name;
                }
            }
            _repository.Flush();
            TriggerSignal();

            return MapToModel(newRecord);
        }

        #endregion

        #region Zone alternates operations

        /// <summary>
        /// Gets display alternate zones for the current request.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Zone> GetCurrentAlternateZones()
        {
            // Get the layers matching the current request.
            IEnumerable<LayerPart> activeLayers = _contentManager.Query<LayerPart, LayerPartRecord>().List();

            var activeLayerIds = new List<int>();
            foreach (var activeLayer in activeLayers)
            {
                // ignore the rule if it fails to execute
                try
                {
                    if (_ruleManager.Matches(activeLayer.Record.LayerRule))
                    {
                        activeLayerIds.Add(activeLayer.ContentItem.Id);
                    }
                }
                catch { }
            }

            // Keep the layers' merged result in cache
            return _cache.Get(string.Format("Frooth.Zones.Layer.{0}", string.Join(".", activeLayerIds.OrderBy(i => i))), 
                ctx => { 
                    MonitorSignal(ctx);
                    return activeLayerIds.SelectMany(GetAlternatesForLayer).Select(c => MapAlternateToZone(c, null)).ToList();
                });
        }

        /// <summary>
        /// Gets alternate zones for the given layer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Zone> GetAlternateZonesForLayer(int layerId)
        {
            // Keep the layers' merged result in cache
            return _cache.Get(string.Format("Frooth.Zones.Layer.{0}", layerId),
                ctx =>
                {
                    MonitorSignal(ctx);
                    return GetAlternatesForLayer(layerId).Select(c => MapAlternateToZone(c, null)).ToList();
                });
        }


        /// <summary>
        /// Retrieves hierarchized collection of zone alternates for the specified layer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ZoneAlternate> GetAlternatesForLayer(int layerId)
        {
            return _cache.Get("Frooth.Alternates.Layer." + layerId, ctx =>
            {
                MonitorSignal(ctx);
                return _altRepository.Fetch(r => r.LayerId == layerId && r.Parent == null)
                    .Select(z => MapHierarchy(z, null)).ToList();
            });
        }

        /// <summary>
        /// Gets flattened list of all zone alterations for given layer.
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns></returns>
        public IEnumerable<ZoneAlternate> GetAlternatesForLayerFlat(int layerId) {
            return GetAlternatesForLayer(layerId).Flatten();
        }

        /// <summary>
        /// Gets all zone alternates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ZoneAlternate> GetAlternates()
        {
            return _cache.Get("Frooth.Alternates.All", ctx =>
            {
                MonitorSignal(ctx);
                return _altRepository.Fetch(r => r.Parent == null)
                    .Select(z => MapHierarchy(z, null)).ToList();
            });
        }

        /// <summary>
        /// Gets the specified alternate.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ZoneAlternate GetZoneAlternate(int id) {
            return MapToModel(_altRepository.Get(id));
        }

        /// <summary>
        /// Gets hierarchized alternates for specified zone.
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns></returns>
        public IEnumerable<ZoneAlternate> GetAlternatesForZone(string zoneName)
        {
            return _altRepository.Fetch(r => r.ZoneName == zoneName)
                    .Select(z => MapHierarchy(z, null)).ToList();
        }

        /// <summary>
        /// Creates new alternate
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="layerId"></param>
        /// <param name="parentId"></param>
        /// <param name="isCollapsible"></param>
        /// <param name="isVertical"></param>
        /// <param name="isRemoved"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ZoneAlternate CreateAlternate(string zone, int layerId, int parentId, bool isCollapsible, bool isVertical, bool isRemoved, string tag)
        {
            var parent = parentId != default(int) ? _altRepository.Get(parentId) : null;
            var siblings = _altRepository.Fetch(z => z.Parent == parent);
            var record = new ZoneAlternateRecord
            {
                ZoneName = zone,
                Parent = parent,
                Tag = tag,
                Position = siblings.Count() > 0 ? siblings.Max(z => z.Position) + 1 : 1,
                IsCollapsible = isCollapsible,
                IsVertical = isVertical,
                IsRemoved = isRemoved,
                LayerId = layerId
            };
            _altRepository.Create(record);
            _altRepository.Flush();
            TriggerSignal();
            return MapToModel(record);
        }

        /// <summary>
        /// Updates the specified alternate
        /// </summary>
        /// <returns></returns>
        public ZoneAlternate UpdateAlternate(ZoneAlternate zone)
        {
            var newRecord = MapFromModel(zone);
            _altRepository.Flush();
            TriggerSignal();
            return MapToModel(newRecord);
        }

        /// <summary>
        /// Deletes specified alternate
        /// </summary>
        /// <param name="alternateId"></param>
        public void DeleteAlternate(int alternateId)
        {
            _altRepository.Delete(_altRepository.Get(alternateId));
            _repository.Flush();
            TriggerSignal();
        }

        /// <summary>
        /// Deletes all alternates for a given zone
        /// </summary>
        /// <param name="zoneName"></param>
        public void DeleteAlternatesForZone(string zoneName)
        {
            foreach (var alt in _altRepository.Fetch(z => z.ZoneName == zoneName)) {
                _altRepository.Delete(alt);
            }
            _altRepository.Flush();
            TriggerSignal();
        }

        /// <summary>
        /// Deletes all alternates for a given layer.
        /// </summary>
        /// <param name="layerId"></param>
        public void DeleteAlternatesForLayer(int layerId)
        {
            foreach (var alt in _altRepository.Fetch(z => z.LayerId == layerId))
            {
                _altRepository.Delete(alt);
            }
            _altRepository.Flush();
            TriggerSignal();
        }

        public void MoveUp(ZoneAlternate zone)
        {
            if (zone == null) {
                return;
            }

            var parent = (zone.Parent != null) ? _altRepository.Get(zone.Parent.Id) : null;
            var alt = _altRepository.Get(zone.Id);
            var upper = _altRepository.Table
                .Where(z => z.Parent == parent && z.Position < alt.Position)
                .OrderByDescending(o => o.Position)
                .FirstOrDefault();

            if (upper == null) {
                return;
            }

            var newPos = upper.Position;
            upper.Position = alt.Position;
            alt.Position = newPos;
            _repository.Flush();
        }

        public void MoveUp(Zone zone) {
            throw new NotImplementedException();
        }

        public void MoveDown(ZoneAlternate zone)
        {
            throw new NotImplementedException();
        }

        public void MoveDown(Zone zone) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Internal methods

        internal IEnumerable<Zone> DefaultZones()
        {
            return _cache.Get("Frooth.Zones.Hierarchy", ctx =>
            {
                MonitorSignal(ctx);
                return _repository.Fetch(r => r.Parent == null)
                    .Select(z => MapHierarchy(z, null)).ToList();
            });
        }

        #region Mapping methods
        
        internal Zone MapHierarchy(ZoneRecord source, Zone useAsParent)
        {
            var zone = MapToModel(source);
            zone.Parent = useAsParent;
            zone.Children = _repository.Fetch(z => z.Parent.Id == zone.Id || z.ParentName == zone.Name).ToList().Select(z => MapHierarchy(z, zone)).ToList();
            return zone;
        }

        internal ZoneAlternate MapHierarchy(ZoneAlternateRecord source, ZoneAlternate useAsParent)
        {
            var zone = MapToModel(source);
            zone.Parent = useAsParent;
            zone.Children = _altRepository.Fetch(z => z.Parent.Id == zone.Id).ToList().Select(z => MapHierarchy(z, zone)).ToList();
            return zone;
        }

        internal Zone MapToModel(ZoneRecord source)
        {
            if (source != null)
            {
                var zone = new Zone
                {
                    Id = source.Id,
                    ParentId = (source.Parent != null) ? (int?)source.Parent.Id : null,
                    ParentName = source.ParentName ?? ((source.Parent != null) ? source.Parent.Name : null),
                    Position = source.Position,
                    IsCollapsible = source.IsCollapsible,
                    Tag = source.Tag,
                    IsVertical = source.IsVertical,
                    IsReadOnly = false,
                    Name = source.Name,
                    Priority = Int32.MaxValue - 1
                };
                return zone;
            }

            return null;
        }

        internal ZoneAlternate MapToModel(ZoneAlternateRecord source)
        {
            if (source != null) {
                var zone = new ZoneAlternate {
                    Id = source.Id,
                    Parent = MapToModel(source.Parent),
                    ZoneName = source.ZoneName,
                    Position = source.Position,
                    Tag = source.Tag,
                    IsCollapsible = source.IsCollapsible,
                    IsVertical = source.IsVertical,
                    IsRemoved = source.IsRemoved,
                    LayerId = source.LayerId,
                    /*Classes = source.Classes.Split(',').Select(s => s.Trim()),
                    Attributes = source.Classes.Split(',')
                                    .ToDictionary(
                                        k => k.Split('=').FirstOrDefault(), 
                                        v => v.Split('=').Skip(1).FirstOrDefault())*/
                };
                return zone;
            }

            return null;
        }

        internal ZoneRecord MapFromModel(Zone source)
        {
            if (source != null)
            {
                var parent = source.ParentId.HasValue
                                 ? _repository.Get(source.ParentId.Value)
                                 : (!string.IsNullOrWhiteSpace(source.ParentName))
                                        ? _repository.Get(z => z.Name == source.ParentName)
                                        : null;

                var record = _repository.Get(source.Id);
                if (record != null)
                {
                    record.Id = source.Id;
                    record.Parent = parent;
                    record.ParentName = source.ParentName;
                    record.Position = source.Position;
                    record.Tag = source.Tag;
                    record.IsCollapsible = source.IsCollapsible;
                    record.IsVertical = source.IsVertical;
                    record.Name = source.Name;
                }
                return record;
            }

            return null;
        }

        internal ZoneAlternateRecord MapFromModel(ZoneAlternate source)
        {
            if (source != null) {
                var parent = source.Parent != null ? _altRepository.Get(source.Parent.Id) : null;
                var record = _altRepository.Get(source.Id);
                var zone = _repository.Get(z => z.Name == source.ZoneName);

                if (record != null)
                {
                    record.Id = source.Id;
                    record.Parent = parent;
                    record.ZoneName = source.ZoneName;
                    record.Position = source.Position;
                    record.Tag = source.Tag;
                    record.IsCollapsible = source.IsCollapsible;
                    record.IsVertical = source.IsVertical;
                    record.IsRemoved = source.IsRemoved;
                    record.LayerId = source.LayerId;
                    record.Classes = string.Join(", ", source.Classes);
                    record.Attributes = string.Join(",", source.Attributes.Select(kv => String.Format("{0}={1}", kv.Key, kv.Value)));
                }
                return record;
            }

            return null;
        }

        internal Zone MapAlternateToZone(ZoneAlternate source, Zone parent) {
            if (source != null) {
                var zone = new Zone {
                    Attributes = source.Attributes,
                    Classes = source.Classes,
                    IsCollapsible = source.IsCollapsible,
                    IsRemoved = source.IsRemoved,
                    IsVertical = source.IsVertical,
                    Name = source.ZoneName,
                    Parent = parent,
                    ParentName = parent != null ? parent.Name : null,
                    Tag = source.Tag,
                    Position = source.Position,
                    Priority = Int32.MaxValue
                };
                zone.Children = source.Children.Select(c => MapAlternateToZone(c, zone)).ToList();
                return zone;
            }

            return null;

        }

        #endregion

        #region Signals

        internal void MonitorSignal(IAcquireContext ctx)
        {
            ctx.Monitor(_signals.When(SignalName));
        }

        internal void TriggerSignal()
        {
            _signals.Trigger(SignalName);
        }

        #endregion

        #endregion

        
    }


}