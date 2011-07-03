using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Szmyd.Frooth.Models;

namespace Szmyd.Frooth.Services
{
    public interface IResourceService : IDependency {
        IEnumerable<Resource> GetStyles();
        IEnumerable<Resource> GetScripts();
        IEnumerable<Resource> GetStylesForLayer(int layerId);
        IEnumerable<Resource> GetScriptsForLayer(int layerId);

        Resource GetResource(int id);

        Resource CreateResource(Resource resource);
        Resource UpdateResource(Resource resource);
        void DeleteResource(int id);

        void AddToLayer(int resourceId, int layerId);
        void RemoveFromLayer(int resourceId, int layerId);


    }
}