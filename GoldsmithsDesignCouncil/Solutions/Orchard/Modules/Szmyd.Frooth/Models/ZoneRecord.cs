using System.Collections.Generic;
using System.Linq;
using Orchard.Data.Conventions;
using Orchard.Environment.Extensions;
using Orchard.Widgets.Models;

namespace Szmyd.Frooth.Models
{
    /// <summary>
    /// Zone entity for storage in db.
    /// </summary>
    public class ZoneRecord
    {
        /// <summary>
        /// Zone ID.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Zone name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Parent name.
        /// </summary>
        public virtual string ParentName { get; set; }

        /// <summary>
        /// Position within siblings.
        /// </summary>
        public virtual int Position { get; set; }

        /// <summary>
        /// True if children are to be vertically placed, false if horizontally.
        /// </summary>
        public virtual bool IsVertical { get; set; }

        /// <summary>
        /// Should this zone be collapsed when no items are present?
        /// </summary>
        public virtual bool IsCollapsible { get; set; }

        /// <summary>
        /// Wrapper tag for this zone.
        /// </summary>
        public virtual string Tag { get; set; }

        /// <summary>
        /// Parent zone (if any)
        /// </summary>
        [CascadeAllDeleteOrphan]
        public virtual ZoneRecord Parent { get; set; }

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