using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using GridMvc.Columns;

namespace GridMvc.Html
{
    public static class GridExtensions
    {
        internal const string DefaultPartialViewName = "_Grid";

        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items)
            where T : class
        {
            return Grid(helper, items, DefaultPartialViewName);
        }

        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items, string viewName)
            where T : class
        {
            return Grid(helper, items, GridRenderOptions.Create(string.Empty, viewName));
        }

        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items,
                                                  GridRenderOptions renderOptions)
            where T : class
        {
            var options = new GridHtmlOptions<T>(items.AsQueryable(), helper.ViewContext, renderOptions.ViewName);
            options.Id = renderOptions.GridId;
            return options;
        }

        //support IHtmlString in RenderValueAs method
        public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, IHtmlString> constraint)
        {
            Func<T, string> valueContraint = a => constraint(a).ToHtmlString();
            return column.RenderValueAs(valueContraint);
        }

        //support WebPages inline helpers
        public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, Func<object, HelperResult>> constraint)
        {
            Func<T, string> valueContraint = a => constraint(a)(null).ToHtmlString();
            return column.RenderValueAs(valueContraint);
        }
    }
}