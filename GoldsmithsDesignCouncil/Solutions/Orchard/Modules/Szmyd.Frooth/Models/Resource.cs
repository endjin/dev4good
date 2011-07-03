using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Szmyd.Frooth.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Resource Dependency { get; set; }
        public ResourceType Type { get; set; }
        public ResourceLocation Location { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public string LocalPath { get; set; }
    }
}