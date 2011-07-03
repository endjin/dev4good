using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Szmyd.Frooth {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageZones = new Permission { Name = "ManageZones", Description = "Manage content zones" };
        public static readonly Permission ManageAlternates = new Permission { Name = "ManageZoneAlternates", Description = "Manage zone alternates for layers" };
        public static readonly Permission ManageResources = new Permission { Name = "ManageStyles", Description = "Manage layout resources" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageZones,
                ManageAlternates,
                ManageResources
             };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageZones, ManageAlternates, ManageResources}
                }
            };
        }
    }
}
