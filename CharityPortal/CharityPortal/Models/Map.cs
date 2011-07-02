using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CharityPortal.Models
{
    public class Map
    {
        /// <summary>
        /// Initializes a new instance of the Map class.
        /// </summary>
        public Map()
        {
            ZoomLevel = 10;
        }

        public int ZoomLevel { get; set; }
        public List<Marker> Markers { get; set; }
    }
}