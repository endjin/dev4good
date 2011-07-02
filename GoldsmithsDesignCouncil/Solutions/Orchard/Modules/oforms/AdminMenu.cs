using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace oforms
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.AddImageSet("users")
                .Add(T("OForms"), "1",
                    menu => menu.Action("Index", "Admin", new { area = "oforms" })
                        .Permission(StandardPermissions.SiteOwner)
                        .Add(T("OForms"), "1.0", item => item.Action("Index", "Admin", new { area = "oforms" })
                            .LocalNav().Permission(StandardPermissions.SiteOwner)));
        }
    }
}