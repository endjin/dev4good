using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace Szmyd.Frooth {
    
    public class Routes : IRouteProvider {
        #region IRouteProvider Members

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                new RouteDescriptor {
                    Priority = 12,
                    Route = new Route(
                        "Admin/Frooth/Zones/{action}",
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"},
                            {"controller", "Zone"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 12,
                    Route = new Route(
                        "Admin/Frooth/Layers/{action}",
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"},
                            {"controller", "Alternate"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"}
                        },
                        new MvcRouteHandler())
                }
                ,new RouteDescriptor {
                    Priority = 12,
                    Route = new Route(
                        "Admin/Frooth/Styles",
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"},
                            {"controller", "Resource"},
                            {"action", "Styles"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"}
                        },
                        new MvcRouteHandler())
                },new RouteDescriptor {
                    Priority = 12,
                    Route = new Route(
                        "Admin/Frooth/Scripts",
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"},
                            {"controller", "Resource"},
                            {"action", "Scripts"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Szmyd.Frooth"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        #endregion
    }
}