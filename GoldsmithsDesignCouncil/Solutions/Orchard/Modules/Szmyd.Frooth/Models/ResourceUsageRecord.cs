namespace Szmyd.Frooth.Models {
    public class ResourceUsageRecord {
        public virtual int Id { get; set; }
        public virtual int LayerId { get; set; }
        public virtual ResourceRecord Resource { get; set; }
    }
}