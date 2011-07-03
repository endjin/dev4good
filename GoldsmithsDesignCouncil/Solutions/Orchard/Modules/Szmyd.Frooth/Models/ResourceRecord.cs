using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Conventions;

namespace Szmyd.Frooth.Models
{
    public class ResourceRecord
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual ResourceRecord Dependency { get; set; }

        public virtual int Type { get; set; }
        public virtual int Location { get; set; }
        public virtual string Url { get; set; }
        public virtual string LocalPath { get; set; }
    }
}