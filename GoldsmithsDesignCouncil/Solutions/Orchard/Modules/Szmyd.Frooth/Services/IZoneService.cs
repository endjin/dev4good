using System;
using System.Collections.Generic;
using Orchard;
using Orchard.Environment.Extensions;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.Services
{
    /// <summary>
    /// Service for providing operations on zones
    /// </summary>
    public interface IZoneService : IDependency
    {
        #region Zone operations
        

        /// <summary>
        /// Retrieves hierarchized (parent/children) collection of existing zones.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Zone> GetZones();

        /// <summary>
        /// Gets specified zone.
        /// </summary>
        /// <param name="zoneId">Id of the zone.</param>
        /// <returns></returns>
        Zone GetZone(int zoneId);

        /// <summary>
        /// Gets specified zone by name.
        /// </summary>
        /// <param name="zoneName">Name of the zone.</param>
        /// <returns></returns>
        Zone GetZone(string zoneName);

        /// <summary>
        /// Deletes specified zone
        /// </summary>
        /// <param name="zoneId"></param>
        void DeleteZone(int zoneId);

        /// <summary>
        /// Creates new zone.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        /// <param name="isCollapsible"></param>
        /// <param name="isVertical"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        Zone CreateZone(string name, string parentName, bool isCollapsible, bool isVertical, string tag);

        /// <summary>
        /// Updates given zone from given dynamic model.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Zone UpdateZone(Zone part);
        #endregion

        #region Zone alternate operations

        /// <summary>
        /// Gets display alternate zones for the current request.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Zone> GetCurrentAlternateZones();

        /// <summary>
        /// Gets alternate zones for the given layer.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Zone> GetAlternateZonesForLayer(int layerId);

        /// <summary>
        /// Retrieves hierarchized collection of zone alternates for the specified layer.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ZoneAlternate> GetAlternatesForLayer(int layerId);

        /// <summary>
        /// Gets flattened list of all zone alterations for given layer.
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns></returns>
        IEnumerable<ZoneAlternate> GetAlternatesForLayerFlat(int layerId);

        /// <summary>
        /// Gets all zone alternates.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ZoneAlternate> GetAlternates();

        /// <summary>
        /// Gets the specified alternate.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ZoneAlternate GetZoneAlternate(int id);

        /// <summary>
        /// Gets hierarchized alternates for specified zone.
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns></returns>
        IEnumerable<ZoneAlternate> GetAlternatesForZone(string zoneName);

        /// <summary>
        /// Creates new alternate
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="parentName"></param>
        /// <param name="layerId"></param>
        /// <param name="parentId"></param>
        /// <param name="isCollapsible"></param>
        /// <param name="isVertical"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        ZoneAlternate CreateAlternate(string zone, int layerId, int parentId, bool isCollapsible, bool isVertical, bool isRemoved, string tag);

        /// <summary>
        /// Updates the specified alternate
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        ZoneAlternate UpdateAlternate(ZoneAlternate part);

        /// <summary>
        /// Deletes specified alternate
        /// </summary>
        /// <param name="alternateId"></param>
        void DeleteAlternate(int alternateId);

        /// <summary>
        /// Deletes all alternates for a given zone
        /// </summary>
        /// <param name="zoneName"></param>
        void DeleteAlternatesForZone(string zoneName);

        /// <summary>
        /// Deletes all alternates for a given layer.
        /// </summary>
        /// <param name="layerId"></param>
        void DeleteAlternatesForLayer(int layerId);

        void MoveUp(ZoneAlternate zone);

        void MoveUp(Zone zone);

        void MoveDown(ZoneAlternate zone);

        void MoveDown(Zone zone);



        #endregion
    }
}
