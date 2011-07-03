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
    public class ZoneAlternate
    {
        public ZoneAlternate()
        {
            Children = Enumerable.Empty<ZoneAlternate>();
        }
        #region Internal IDs for storage purposes

        /// <summary>
        /// Zone ID for database storage
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region General properties

        /// <summary>
        /// Which zone is it an alternate for?
        /// </summary>
        public string ZoneName { get; set; }

        /// <summary>
        /// Wrapper tag for this zone alternate
        /// </summary>
        public string Tag { get; set; }

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

        public bool IsRemoved { get; set; }

        private bool _isReadOnly = true;

        /// <summary>
        /// Can be changed.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        /// <summary>
        /// Position within parent's children.
        /// </summary>
        [Required]
        public int Position { get; set; }

        /// <summary>
        /// Additional CSS classes
        /// </summary>
        public virtual IEnumerable<string> Classes { get; set; }

        /// <summary>
        /// Additional attributes
        /// </summary>
        public virtual IDictionary<string, string> Attributes { get; set; }

        #endregion

        #region Hierarchy-related properties

        /// <summary>
        /// Child zones
        /// </summary>
        public IEnumerable<ZoneAlternate> Children { get; set; }

        /// <summary>
        /// Parent zone
        /// </summary>
        public ZoneAlternate Parent { get; set; }

        /// <summary>
        /// Id of a layer this zone alternate belongs to.
        /// </summary>
        public int LayerId { get; set; }


        #endregion
    }
}