//using System;
//using Orchard.Commands;
//using Orchard.ContentManagement;
//using Orchard.ContentManagement.Aspects;
//using Orchard.Security;
//using Orchard.Settings;
//using Orchard.Widgets.Models;

//namespace Szmyd.Frooth.Commands {
//    public class ZoneCommands : DefaultOrchardCommandHandler {
//        private readonly IContentManager _contentManager;
//        private readonly ISiteService _siteService;
//        private readonly IMembershipService _membershipService;

//        public ZoneCommands(IContentManager contentManager, ISiteService siteService, IMembershipService membershipService) {
//            _contentManager = contentManager;
//            _siteService = siteService;
//            _membershipService = membershipService;
//        }

//        [OrchardSwitch]
//        public string LayerRule { get; set; }

//        [OrchardSwitch]
//        public string Description { get; set; }

//        [OrchardSwitch]
//        public string Owner { get; set; }

//        [CommandName("zone create")]
//        [CommandHelp("zone create <name> /LayerRule:<rule> [/Description:<description>] [/Owner:<owner>]\r\n\t" + "Creates a new layer")]
//        [OrchardSwitches("LayerRule,Description,Owner")]
//        public void Create(string name) {
//            Context.Output.WriteLine(T("Creating Zone {0}", name));

//            IContent layer = _contentManager.Create<LayerPart>("Layer", t => {
//                                                                            t.Record.Name = name; 
//                                                                            t.Record.LayerRule = LayerRule;
//                                                                            t.Record.Description = Description ?? String.Empty;
//                                                                        });

//            _contentManager.Publish(layer.ContentItem);
//            if (String.IsNullOrEmpty(Owner)) {
//                Owner = _siteService.GetSiteSettings().SuperUser;
//            }
//            var owner = _membershipService.GetUser(Owner);
//            layer.As<ICommonPart>().Owner = owner;

//            Context.Output.WriteLine(T("Layer created successfully.").Text);
//        }
//    }
//}