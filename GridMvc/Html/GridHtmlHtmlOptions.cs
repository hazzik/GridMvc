using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GridMvc.Html
{
    /// <summary>
    /// Grid adapter for html helper
    /// </summary>
    public class GridHtmlHtmlOptions<T> : Grid<T>, IGridHtmlOptions where T : class
    {
        private readonly ViewContext _viewContext;

        public GridHtmlHtmlOptions(IQueryable<T> items, ViewContext viewContext, string viewName)
            : base(items)
        {
            _viewContext = viewContext;
            GridViewName = viewName;
        }

        public string GridViewName { get; set; }

        #region IGridHtmlOptions Members

        public string ToHtmlString()
        {
            return RenderPartialViewToString(GridViewName, this, _viewContext);
        }

        public IGridHtmlOptions WithPaging(int pageSize)
        {
            EnablePaging = true;
            Pager.PageSize = pageSize;
            return this;
        }

        public IGridHtmlOptions WithPaging(int pageSize, int maxDisplayedItems)
        {
            EnablePaging = true;
            Pager.PageSize = pageSize;
            Pager.MaxDisplayedPages = maxDisplayedItems;
            return this;
        }

        public IGridHtmlOptions EmptyText(string text)
        {
            EmptyGridText = text;
            return this;
        }

        #endregion

        private static string RenderPartialViewToString(string viewName, object model, ViewContext viewContext)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName");

            var context = new ControllerContext(viewContext.RequestContext, viewContext.Controller);
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var newViewContext = new ViewContext(context, viewResult.View, viewContext.ViewData,
                                                     viewContext.TempData, sw)
                                         {
                                             ViewData =
                                                 {
                                                     Model = model
                                                 }
                                         };
                viewResult.View.Render(newViewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}