using Orchard.ContentManagement.Handlers;
using oforms.Models;
using Orchard.Data;

namespace oforms.Handlers
{
    public class OFormHandler : ContentHandler
    {
        public OFormHandler(IRepository<OFormPartRecord> repo)
        {
            Filters.Add(new ActivatingFilter<OFormPart>("OForm"));
            Filters.Add(StorageFilter.For(repo));
        }
    }
}