using Orchard;

namespace Szmyd.Frooth.Providers
{
    /// <summary>
    /// Extension point for delivering hardcoded zone definitions from other modules/themes.
    /// Zones defined by IZoneProviders are shown in Dashboard, but cannot be modified in any way.
    /// It is a feature replacing the default hardcoding of zones in Layout.cshtml shape file.
    /// </summary>
    public interface IZonesProvider : IDependency {

        /// <summary>
        /// Zone overriding order (higher overrides lower).
        /// </summary>
        /// <remarks>
        /// Use values >5, as the lower numbers are reserved for default providers. 
        /// Otherwise you can get unpredictable results.
        /// </remarks>
        int Priority { get; }

        /// <summary>
        /// Builds the zone hierarchy using the provided builder.
        /// </summary>
        /// <returns></returns>
        void BuildZones(ZoneBuilder builder);
    }
}
