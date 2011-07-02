using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oforms.Models
{
    public class OFormFileRecord
    {
        public virtual int Id { get; set; }

        public virtual OFormResultRecord OFormResultRecord { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string OriginalName { get; set; }

        public virtual string ContentType { get; set; }

        public virtual byte[] Bytes { get; set; }

        public virtual long Size { get; set; }
    }
}