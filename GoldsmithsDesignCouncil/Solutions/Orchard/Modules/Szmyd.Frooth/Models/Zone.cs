using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Szmyd.Frooth.Models
{
    /// <summary>
    /// Zone POCO entity for caching and general usage.
    /// </summary>
    public class Zone
    {
        public Zone() {
            Children = new List<Zone>();
            Alternates = new Dictionary<int, Zone>();
        }
        #region Internal IDs for storage purposes

        /// <summary>
        /// Zone ID for database storage
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Parent zone Id.
        /// </summary>
        internal int? ParentId { get; set; }

        #endregion

        #region General properties
        
        /// <summary>
        /// Zone name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Whether to place zone vertically (horizontally if false).
        /// </summary>
        [Required]
        public bool IsVertical { get; set; }

        /// <summary>
        /// Should zone collapse if there's no other content than empty zones inside.
        /// </summary>
        [Required]
        public bool IsCollapsible { get; set; }

        private bool _isReadOnly = true;

        /// <summary>
        /// Can be changed.
        /// </summary>
        public bool IsReadOnly {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        /// <summary>
        /// Position within parent's children.
        /// </summary>
        [Required]
        public int Position { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// Additional CSS classes
        /// </summary>
        public IEnumerable<string> Classes { get; set; }

        /// <summary>
        /// Additional attributes
        /// </summary>
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Should this zone be removed from display?
        /// </summary>
        public bool IsRemoved { get; set; }

        #endregion

        #region Hierarchy-related properties

        /// <summary>
        /// Parent zone name.
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Child zones
        /// </summary>
        public IEnumerable<Zone> Children { get; set; }

        /// <summary>
        /// Alternate zone settings for different layers.
        /// </summary>
        public IDictionary<int, Zone> Alternates { get; set; }

        /// <summary>
        /// Parent zone
        /// </summary>
        public Zone Parent { get; set; }

        /// <summary>
        /// Is leaf zone.
        /// </summary>
        public bool IsLeaf
        {
            get { return Children == null || Children.Count() == 0; }
        }

        /// <summary>
        /// Priority in which the shapes will be overridden.
        /// </summary>
        internal int Priority { get; set; }

        #endregion
    }
}