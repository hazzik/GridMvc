using System.Collections.Generic;
using System.Web.Mvc;
using GridMvc.Columns;
using GridMvc.Pagination;

namespace GridMvc.Html
{
    ///// <summary>
    /////     Grid adapter for html helper
    ///// </summary>
    //public class HtmlGrid1<T> : Grid<T>, IHtmlString where T : class
    //{
    //    private readonly ViewContext _viewContext;

    //    public HtmlGrid(IQueryable<T> items, ViewContext viewContext, string viewName)
    //        : base(items)
    //    {
    //        _viewContext = viewContext;
    //        GridViewName = viewName;
    //    }

    //    public string GridViewName { get; set; }

    //    public string ToHtmlString()
    //    {
    //        return GridExtensions.RenderPartialViewToString(GridViewName, this, _viewContext);
    //    }

    //}

    /// <summary>
    ///     Grid adapter for html helper
    /// </summary>
    public class HtmlGrid<T> : GridHtmlOptions<T>, IGrid where T : class
    {
        private readonly Grid<T> _source;


        public HtmlGrid(Grid<T> source, ViewContext viewContext, string viewName)
            : base(source, viewContext, viewName)
        {
            _source = source;
        }

        public GridRenderOptions RenderOptions
        {
            get { return _source.RenderOptions; }
        }

        IGridColumnCollection IGrid.Columns
        {
            get { return _source.Columns; }
        }

        IEnumerable<object> IGrid.ItemsToDisplay
        {
            get { return (_source as IGrid).ItemsToDisplay; }
        }

        int IGrid.ItemsCount
        {
            get { return _source.ItemsCount; }
            set { _source.ItemsCount = value; }
        }

        int IGrid.DisplayingItemsCount
        {
            get { return _source.DisplayingItemsCount; }
        }

        IGridPager IGrid.Pager
        {
            get { return _source.Pager; }
        }

        bool IGrid.EnablePaging
        {
            get { return _source.EnablePaging; }
        }

        string IGrid.EmptyGridText
        {
            get { return _source.EmptyGridText; }
        }

        string IGrid.Language
        {
            get { return _source.Language; }
        }

        public ISanitizer Sanitizer
        {
            get { return _source.Sanitizer; }
        }

        string IGrid.GetRowCssClasses(object item)
        {
            return _source.GetRowCssClasses(item);
        }

        IGridSettingsProvider IGrid.Settings
        {
            get { return _source.Settings; }
        }

        void IGrid.OnPreRender()
        {
            _source.OnPreRender();
        }
    }
}