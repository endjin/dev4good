using System;
using Orchard.Data.Conventions;
using System.Collections.Generic;

namespace oforms.Models
{
    public class OFormResultRecord
    {
        public OFormResultRecord()
        {
            Files = new List<OFormFileRecord>();
        }

        public virtual int Id { get; set; }

        public virtual OFormPartRecord OFormPartRecord { get; set; }

        [StringLengthMax]
        public virtual string Xml { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string Ip { get; set; }

        public virtual IList<OFormFileRecord> Files { get; set; }

        public virtual void AddFile(OFormFileRecord formFile)
        {
            formFile.OFormResultRecord = this;
            Files.Add(formFile);
        }
    }
}