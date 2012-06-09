using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GridMvc.Columns;

namespace GridMvc.Html
{
    public static class GridExtensions
    {
        private const string DefaultPartialViewName = "_Grid";

        public static IGridHtmlOptions Grid<T>(this HtmlHelper helper, Action<IGridColumnCollection<T>> columnBuilder)
            where T : class
        {
            return Grid(helper, columnBuilder, DefaultPartialViewName);
        }

        public static IGridHtmlOptions Grid<T>(this HtmlHelper helper, Action<IGridColumnCollection<T>> columnBuilder,
                                               string viewName) where T : class
        {
            //get grid items from current model:
            var items = helper.ViewContext.ViewData.Model as IEnumerable<T>;
            if (items == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Your current Model is of type '{0}', but Grid.Mvc required Model of type '{1}'. Or you can specify your collection in other overload method.",
                        helper.ViewContext.ViewData.Model.GetType(),
                        string.Format("IEnumerable<{0}>", helper.ViewContext.ViewData.Model)));
            }
            return Grid(helper, columnBuilder, items);
        }

        public static IGridHtmlOptions Grid<T>(this HtmlHelper helper, Action<IGridColumnCollection<T>> columnBuilder,
                                               IEnumerable<T> gridItems) where T : class
        {
            return Grid(helper, columnBuilder, gridItems, DefaultPartialViewName);
        }

        public static IGridHtmlOptions Grid<T>(this HtmlHelper helper, Action<IGridColumnCollection<T>> columnBuilder,
                                               IEnumerable<T> gridItems, string viewName) where T : class
        {
            var options = new GridHtmlHtmlOptions<T>(gridItems.AsQueryable(), helper.ViewContext, viewName);
            columnBuilder(options.Columns);
            return options;
        }
    }
}