using System;
using System.Collections.Generic;
using System.Linq;

namespace CharityPortal.Models
{
    public class OverviewMapViewModel
    {
        /// <summary>
        /// Initializes a new instance of the OverviewMapViewModel class.
        /// </summary>
        public OverviewMapViewModel()
        {
            Map = new Map();
        }

        public Map Map { get; set; }
    }
}
