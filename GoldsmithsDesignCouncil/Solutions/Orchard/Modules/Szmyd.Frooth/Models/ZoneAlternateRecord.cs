using System.Collections.Generic;
using Orchard.Data.Conventions;
using Orchard.Environment.Extensions;

namespace Szmyd.Frooth.Models
{
    /// <summary>
    /// Record for persisting info about zone alternate for specific layer.
    /// </summary>
    public class ZoneAlternateRecord
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Name of the zone this record is an alternate to.
        /// </summary>
        public virtual string ZoneName { get; set; }

        /// <summary>
        /// Position within siblings.
        /// </summary>
        public virtual int Position { get; set; }

        /// <summary>
        /// True if children are to be vertically placed, false if horizontally.
        /// </summary>
        public virtual bool IsVertical { get; set; }

        /// <summary>
        /// Should this zone alternate be collapsed when no items are present?
        /// </summary>
        public virtual bool IsCollapsible { get; set; }

        /// <summary>
        /// ID of layer this alternate belongs to
        /// </summary>
        public virtual int LayerId { get; set; }

        /// <summary>
        /// Wrapper tag for this zone.
        /// </summary>
        public virtual string Tag { get; set; }

        /// <summary>
        /// Whether the alternate is to not show this zone.
        /// If true, the layer won't be displayed at all (even if it has some content inside).
        /// </summary>
        public virtual bool IsRemoved { get; set; }

        /// <summary>
        /// Parent zone (if any)
        /// </summary>
        [CascadeAllDeleteOrphan]
        public virtual ZoneAlternateRecord Parent { get; set; }

        /// <summary>
        /// Additional CSS classes
        /// </summary>
        public virtual string Classes { get; set; }

        /// <summary>
        /// Additional attributes
        /// </summary>
        public virtual string Attributes { get; set; }
    }
}