using System;
using System.Collections.Generic;
using System.Linq;

namespace CharityPortal.Models
{
    public class Marker
    {
        public string Title { get; set; }
        public string IconUrl { get; set; }

        /// <summary>
        /// Html or plaintext
        /// </summary>
        public string Content { get; set; }

        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
