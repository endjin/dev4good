using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Widgets.Models;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.ViewModels
{
    public class ResourcesViewModel
    {
        public int LayerId { get; set; }
        public ResourceType Type { get; set; }
        public IEnumerable<Resource> AvailableStyles { get; set; }
        public IEnumerable<Resource> AvailableScripts { get; set; }
        public IEnumerable<Resource> ScriptsInLayer { get; set; }
        public IEnumerable<Resource> StylesInLayer { get; set; }
        public IEnumerable<LayerPart> Layers { get; set; }
        public Resource New { get; set; }
    }
}