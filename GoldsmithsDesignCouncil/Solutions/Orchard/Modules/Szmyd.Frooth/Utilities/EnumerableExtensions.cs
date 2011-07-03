using System.Collections.Generic;
using System.Linq;
using Szmyd.Frooth.Models;
using Zone = Szmyd.Frooth.Models.Zone;

namespace Szmyd.Frooth.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Zone> Flatten(this IEnumerable<Zone> source)
        {
            return source.Concat(source.SelectMany(c => c.Children.Flatten()));
        }

        public static IEnumerable<ZoneAlternate> Flatten(this IEnumerable<ZoneAlternate> source)
        {
            return source.Concat(source.SelectMany(c => c.Children.Flatten()));
        }

        public static IEnumerable<dynamic> Flatten(this IEnumerable<dynamic> source)
        {
            return source.Concat(source.SelectMany(c => ((IEnumerable<dynamic>)c).Flatten()));
        }


    }
}