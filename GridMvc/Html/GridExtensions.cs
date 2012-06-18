using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
        // Action<IGridColumnCollection<T>> columnBuilder
        public static IGridHtmlOptions<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items, string viewName) where T : class
        {
            var options = new GridHtmlOptions<T>(items.AsQueryable(), helper.ViewContext, viewName);
            return options;
        }
    }
}