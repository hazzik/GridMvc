using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GridMvc.Columns;
using GridMvc.Pagination;

namespace GridMvc.Html
{
    /// <summary>
    ///     Grid adapter for html helper
    /// </summary>
    public class GridHtmlOptions<T> : Grid<T>, IGridHtmlOptions<T> where T : class
    {
        private readonly ViewContext _viewContext;

        public GridHtmlOptions(IQueryable<T> items, ViewContext viewContext, string viewName)
            : base(items)
        {
            _viewContext = viewContext;
            GridViewName = viewName;
        }

        public string GridViewName { get; set; }

        #region IGridHtmlOptions<T> Members

        public string ToHtmlString()
        {
            return RenderPartialViewToString(GridViewName, this, _viewContext);
        }


        public new IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder)
        {
            columnBuilder(base.Columns);
            return this;
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize)
        {
            EnablePaging = true;
            Pager.PageSize = pageSize;
            return this;
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems)
        {
            EnablePaging = true;
            Pager.PageSize = pageSize;
            Pager.MaxDisplayedPages = maxDisplayedItems;
            return this;
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName)
        {
            EnablePaging = true;
            Pager.PageSize = pageSize;
            Pager.MaxDisplayedPages = maxDisplayedItems;
            ((GridPager) Pager).ParameterName = queryStringParameterName;
            return this;
        }

        public IGridHtmlOptions<T> Sortable()
        {
            return Sortable(true);
        }

        public IGridHtmlOptions<T> Sortable(bool enable)
        {
            DefaultSortEnabled = enable;
            foreach (IGridColumn column in base.Columns)
            {
                var typedColumn = column as IGridColumn<T>;
                if (typedColumn == null) continue;
                typedColumn.Sortable(enable);
            }
            return this;
        }

        public IGridHtmlOptions<T> Filterable()
        {
            return Filterable(true);
        }

        public IGridHtmlOptions<T> Filterable(bool enable)
        {
            DefaultFilteringEnabled = enable;
            foreach (IGridColumn column in base.Columns)
            {
                var typedColumn = column as IGridColumn<T>;
                if (typedColumn == null) continue;
                typedColumn.Filterable(enable);
            }
            return this;
        }

        public IGridHtmlOptions<T> EmptyText(string text)
        {
            EmptyGridText = text;
            return this;
        }

        public IGridHtmlOptions<T> SetLanguage(string lang)
        {
            Language = lang;
            return this;
        }

        public IGridHtmlOptions<T> SetRowCssClasses(Func<T, string> contraint)
        {
            SetRowCssClassesContraint(contraint);
            return this;
        }

        /// <summary>
        ///     Generates columns for all properties of the model.
        ///     Use data annotations to customize columns
        /// </summary>
        public new IGridHtmlOptions<T> AutoGenerateColumns()
        {
            base.AutoGenerateColumns();
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