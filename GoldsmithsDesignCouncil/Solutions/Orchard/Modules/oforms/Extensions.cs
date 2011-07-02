using System.Collections.Generic;
using System.Collections.Specialized;

namespace oforms
{
    public static class OFormGlobals
    {
        public const string CreatedDateKey = "oforms.createdDate";
        public const string IpKey = "oforms.ip";
        public const string NameKey = "oforms.name";
        public const string CaptchaKey = "oforms.captcha";
    }


    public static class Extensions
    {
        public static void CopyTo(this NameValueCollection source, IDictionary<string, string> destination)
        {
            foreach (string key in source.Keys)
            {
                if (destination.ContainsKey(key))
                {
                    destination[key] += ", " + source[key];
                }
                else
                {
                    destination.Add(key, source[key]);
                }
            }
        }
    }
}