using System;
using System.Collections.Generic;
using System.Linq;
using Szmyd.Frooth.Models;
using Szmyd.Frooth.Utilities;

namespace Szmyd.Frooth.Providers {
    /// <summary>
    /// Builder class for creating Zone hierarchy.
    /// </summary>
    public class ZoneBuilder {
        private readonly string _parentName;
        private readonly int _priority;


        internal ZoneBuilder()
            : this(0) {}

        internal ZoneBuilder(int priority)
            : this(new Zone {Name = null, ParentName = null}, priority) {}

        internal ZoneBuilder(Zone parentZone, int priority) {
            _priority = priority;
            ParentZone = parentZone;
            _parentName = parentZone.Name;
        }

        internal Zone ParentZone { get; set; }

        /// <summary>
        /// Creates new zone.
        /// </summary>
        /// <param name="name">The name of zone.</param>
        /// <param name="isCollapsible">Should zone collapse if empty?</param>
        /// <param name="isVertical">Should be vertically placed?</param>
        /// <param name="childrenBuilder">Builder for this zone children.</param>
        /// <param name="tag">Tag wrapping the zone contents.</param>
        /// <returns></returns>
        public ZoneBuilder Add(
            string name,
            Action<ZoneBuilder> childrenBuilder = null,
            bool isCollapsible = true,
            bool isVertical = false,
            string tag = "div") {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("Zone name cannot be empty.");
            }

            if (!Html5.Tags.Contains(tag)) {
                throw new ArgumentException(String.Format("Tag name {0} is invalid. Allowed values: {1}", tag, string.Join(", ", Html5.Tags)));
            }

            var zone = new Zone {
                ParentName = _parentName,
                Name = name,
                IsCollapsible = isCollapsible,
                IsReadOnly = true,
                IsVertical = isVertical,
                Priority = _priority,
                Tag = tag,
                Children = new List<Zone>()
            };

            var builder = new ZoneBuilder(zone, _priority);
            if (childrenBuilder != null) {
                childrenBuilder(builder);
            }

            ((IList<Zone>) ParentZone.Children).Add(zone);

            return this;
        }
    }

    ///// <summary>
    ///// Builder class for creating alternate layouts for different layers
    ///// </summary>
    //public class AlternateBuilder
    //{

    //    private IDictionary<string, ZoneAlternateBuilder> _builders;

    //    /// <summary>
    //    /// Layer, for which create the alternate
    //    /// </summary>
    //    /// <param name="layerName">Layer name for this alternates.</param>
    //    /// <returns></returns>
    //    public ZoneAlternateBuilder ForLayer(string layerName)
    //    {
    //        var childBuilder = new ZoneAlternateBuilder(layerName);
    //        _builders[layerName] = childBuilder;
    //        return childBuilder;
    //    }
    //}

    ///// <summary>
    ///// Builder class for creating layer alternate hierarchy.
    ///// </summary>
    //public class ZoneAlternateBuilder
    //{
    //    private readonly string _layerName;

    //    public ZoneAlternateBuilder(string layerName)
    //    {
    //        _layerName = layerName;
    //    }

    //    public ZoneAlternateBuilder Add(
    //        string name,
    //        bool isCollapsible = true,
    //        bool isVertical = false,
    //        Action<ZoneAlternateBuilder> childrenBuilder = null)
    //    {
    //        return this;
    //    }

    //    /// <summary>
    //    /// Marks specified zone as explicitly removed.
    //    /// It won't show even if it has items.
    //    /// </summary>
    //    /// <param name="name">Zone name</param>
    //    /// <returns></returns>
    //    public ZoneAlternateBuilder Remove(string name)
    //    {
    //        return this;
    //    }
    //}
}