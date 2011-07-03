using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CharityPortal.Models;
using CharityPortal.Data;
using System.Web.Mvc;

namespace CharityPortal.Utils
{
    public static class MapUtils
    {
        public static Marker CreateRequiredResourceMarker(Resource resource, UrlHelper url)
        {
            Marker marker = CreateMarker(resource);
            marker.IconUrl = url.Content("~/Content/images/required-resource.png");
            return marker;
        }

        public static Marker CreateAvailableResourceMarker(Resource resource, UrlHelper url)
        {
            Marker marker = CreateMarker(resource);
            marker.IconUrl = url.Content("~/Content/images/available-resource.png");
            return marker;
        }

        private static Marker CreateMarker(Resource resource)
        {
            return new Marker() {
                Title = resource.Title,
                Content = String.Format("<h3>{0}</h3>{1}", resource.Title, resource.Description),
                Latitude = resource.Location.Latitude,
                Longitude = resource.Location.Longitude,
                Address = resource.Location.Address
            };
        }

        public static Marker CreateProjectMarker(Project project, UrlHelper url)
        {
            return new Marker() {
                Title = project.Name,
                Content = String.Format("<h3>{0}</h3>{1}", project.Name, project.Description),
                IconUrl = url.Content("~/Content/images/project.png"),
                Latitude = project.Location.Latitude,
                Longitude = project.Location.Longitude,
                Address = project.Location.Address
            };
        }
    }
}