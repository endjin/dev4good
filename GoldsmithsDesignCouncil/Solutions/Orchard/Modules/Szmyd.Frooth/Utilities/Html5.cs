using System.Collections.Generic;

namespace Szmyd.Frooth.Utilities
{
    public static class Html5
    {
        public static IEnumerable<string> Tags
        {
            get { return new[] {"div", "section", "nav", "article", "aside", "hgroup", "header", "footer", "address"}; }
        }
    }
}