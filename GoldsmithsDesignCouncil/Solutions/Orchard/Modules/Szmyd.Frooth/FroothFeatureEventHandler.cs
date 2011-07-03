using System.Linq;
using JetBrains.Annotations;
using Orchard.Environment;
using Orchard.Environment.Extensions.Models;
using Orchard.Logging;
using Orchard.Media.Services;

namespace Szmyd.Frooth
{
    [UsedImplicitly]
    public class FroothFeatureEventHandler : IFeatureEventHandler
    {
        private readonly IMediaService _services;

        public FroothFeatureEventHandler(IMediaService services)
        {
            _services = services;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Install(Feature feature) { }

        public void Enable(Feature feature) { }

        public void Disable(Feature feature) { }

        public void Uninstall(Feature feature) { }

        public void Installing(Feature feature) { }

        public void Installed(Feature feature){}

        public void Enabling(Feature feature){}

        public void Enabled(Feature feature) {
            var folder = _services.GetMediaFolders("").Where(f => f.Name == "Frooth");
            if(folder.Count() == 0) {
                _services.CreateFolder("", "Frooth");
            }
        }

        public void Disabling(Feature feature){}

        public void Disabled(Feature feature){}

        public void Uninstalling(Feature feature){}

        public void Uninstalled(Feature feature){}
    }
}