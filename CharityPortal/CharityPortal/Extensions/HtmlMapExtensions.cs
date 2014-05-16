using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CharityPortal.Models;

namespace CharityPortal.Extensions
{
    public static class HtmlMapExtensions
    {
        public static MvcHtmlString Map<TModel>(this HtmlHelper<TModel> html, Map map)
        {
            return html.Partial("MapControl", map);
        }
    }
}