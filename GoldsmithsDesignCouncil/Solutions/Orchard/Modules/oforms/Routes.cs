using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace oforms {
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
													Route = new Route(
                                                         "oforms/{name}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "oforms"},
                                                                                      {"controller", "Home"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "oforms"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
							new RouteDescriptor {
													Route = new Route(
                                                         "Admin/oforms/{template}/Create",
                                                         new RouteValueDictionary {
                                                                                      {"area", "oforms"},
                                                                                      {"controller", "Admin"},
                                                                                      {"action", "Create"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "oforms"},
                                                                                      {"controller", "Admin"},
                                                                                      {"action", "Create"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
            };
        }
    }
}