using Orchard.Environment.Extensions;
using Zone = Orchard.UI.Zone;

namespace Szmyd.Frooth.Shapes
{
    /// <summary>
    /// Base class for the "Zone" shape, replacing the default Zone class.
    /// </summary>
    
    public class NestableZone : Zone
    {
        /// <summary>
        /// Zone object connected with this shape
        /// </summary>
        public Models.Zone Current { get; set; }

        /// <summary>
        /// Should this shape collapse?
        /// </summary>
        public bool ShouldCollapse { get; set; }
    }
}