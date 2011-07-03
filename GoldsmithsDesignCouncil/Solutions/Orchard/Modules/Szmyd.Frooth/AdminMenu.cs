using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Szmyd.Frooth
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {

            builder
                .Add(T("Frooth Platform"), "5",
                     menu => menu
                                 .Add(T("Main layout"), "0", item => item.Action("Index", "Zone", new { area = "Szmyd.Frooth" })
                                                                    .Permission(Permissions.ManageZones))
                                 .Add(T("Alternate layouts"), "1", item => item.Action("Index", "Alternate", new { area = "Szmyd.Frooth" })
                                                                    .Permission(Permissions.ManageZones))
                                 .Add(T("Resources"), "2", item => item.Action("Index", "Resource", new {area = "Szmyd.Frooth"})
                                                                     .Permission(Permissions.ManageResources)));
                ;
        }
    }
}
