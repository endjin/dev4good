using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Caching;
using Orchard.Data;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.Services
{
    public class FroothResourceService : IResourceService
    {

        public const string SignalName = "Frooth.Resources.Changed";

        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IRepository<ResourceRecord> _recordRepo;
        private readonly IRepository<ResourceUsageRecord> _usageRepo;

        public FroothResourceService(ICacheManager cacheManager, ISignals signals, IRepository<ResourceRecord> recordRepo, IRepository<ResourceUsageRecord> usageRepo)
        {
            _cacheManager = cacheManager;
            _signals = signals;
            _recordRepo = recordRepo;
            _usageRepo = usageRepo;
        }

        #region Implementation of IResourceService

        public IEnumerable<Resource> GetStyles()
        {
            return _cacheManager.Get("Frooth.Styles", ctx =>
            {
                MonitorSignal(ctx);
                return _recordRepo
                    .Fetch(r => r.Type == (int)ResourceType.Stylesheet)
                    .Select(MapToModel).ToList();
            });
        }

        public IEnumerable<Resource> GetScripts()
        {
            return _cacheManager.Get("Frooth.Scripts", ctx =>
            {
                MonitorSignal(ctx);
                return _recordRepo
                    .Fetch(r => r.Type == (int)ResourceType.Script)
                    .Select(MapToModel).ToList();
            });
        }

        public IEnumerable<Resource> GetStylesForLayer(int layerId)
        {
            return _cacheManager.Get("Frooth.Styles.Layer." + layerId, ctx =>
            {
                MonitorSignal(ctx);
                return _usageRepo
                    .Fetch(r => r.LayerId == layerId && r.Resource.Type == (int)ResourceType.Stylesheet)
                    .Select(r => MapToModel(r.Resource)).ToList();
            });
        }

        public IEnumerable<Resource> GetScriptsForLayer(int layerId)
        {
            return _cacheManager.Get("Frooth.Scripts.Layer." + layerId, ctx =>
            {
                MonitorSignal(ctx);
                return _usageRepo
                    .Fetch(r => r.LayerId == layerId && r.Resource.Type == (int)ResourceType.Script)
                    .Select(r => MapToModel(r.Resource)).ToList();
            });
        }

        public Resource GetResource(int id) {
            return MapToModel(_recordRepo.Get(id));
        }

        public Resource CreateResource(Resource resource) {
            var record = MapFromModel(resource);
            _recordRepo.Create(record);
            _recordRepo.Flush();
            TriggerSignal();

            return MapToModel(record);
        }

        public Resource UpdateResource(Resource resource)
        {
            var record = MapFromModel(resource);
            _recordRepo.Update(record);
            _recordRepo.Flush();
            TriggerSignal();

            return MapToModel(record);
        }

        public void DeleteResource(int id) {
            var record = _recordRepo.Get(id);
            if (record == null) {
                return;
            }
            _recordRepo.Delete(record);
            _recordRepo.Flush();
            TriggerSignal();
        }

        public void AddToLayer(int resourceId, int layerId)
        {
            _usageRepo.Create(
                new ResourceUsageRecord
                {
                    LayerId = layerId,
                    Resource = _recordRepo.Get(resourceId)
                });
            _usageRepo.Flush();
            TriggerSignal();
        }

        public void RemoveFromLayer(int resourceId, int layerId)
        {
            _usageRepo.Delete(_usageRepo.Get(r => r.LayerId == layerId && r.Resource.Id == resourceId));
            _usageRepo.Flush();
            TriggerSignal();
        }

        internal Resource MapToModel(ResourceRecord record)
        {
            if (record != null)
            {
                var res = new Resource
                {
                    Id = record.Id,
                    LocalPath = record.LocalPath,
                    Location = (ResourceLocation)record.Location,
                    Name = record.Name,
                    Url = record.Url,
                    Type = (ResourceType)record.Type,
                    Dependency = MapToModel(record.Dependency)
                };

                return res;
            }

            return null;
        }

        internal ResourceRecord MapFromModel(Resource source)
        {
            if (source != null) {
                var record = source.Id != default(int) ? _recordRepo.Get(source.Id) : new ResourceRecord();

                record.Url = source.Url;
                record.Type = (int) source.Type;
                record.Name = source.Name;
                record.Location = (int) source.Location;
                record.LocalPath = source.LocalPath;
                record.Dependency = MapFromModel(source.Dependency);

                return record;
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
    }
}