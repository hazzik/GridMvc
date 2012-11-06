using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridMvc.Columns;

namespace GridMvc.Html
{
    public static class GridExtensions
    {
        private const string DefaultPartialViewName = "_Grid";

        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items)
            where T : class
        {
            return Grid(helper, items, DefaultPartialViewName);
        }

        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items, string viewName)
            where T : class
        {
            var options = new GridHtmlOptions<T>(items.AsQueryable(), helper.ViewContext, viewName);
            return options;
        }

        //support IHtmlString in RenderValueAs method
        public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, IHtmlString> constraint)
        {
            Func<T, string> valueContraint = a => constraint(a).ToHtmlString();
            return column.RenderValueAs(valueContraint);
        }
    }
}