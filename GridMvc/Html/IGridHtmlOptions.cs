using System;
using System.Web;
using GridMvc.Columns;

namespace GridMvc.Html
{
    /// <summary>
    /// Grid options for html helper
    /// </summary>
    public interface IGridHtmlOptions<T> : IGrid, IHtmlString
    {
        IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder);
        /// <summary>
        /// Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        IGridHtmlOptions<T> WithPaging(int pageSize);

        /// <summary>
        /// Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <param name="maxDisplayedItems">Setup max count of displaying pager links</param>
        IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems);

        /// <summary>
        /// Enable sorting for all columns
        /// </summary>
        IGridHtmlOptions<T> Sortable();

        /// <summary>
        /// Enable or disable sorting for all columns
        /// </summary>
        IGridHtmlOptions<T> Sortable(bool enable);

        /// <summary>
        /// Enable filtering for all columns
        /// </summary>
        IGridHtmlOptions<T> Filterable();

        /// <summary>
        /// Enable or disable filtering for all columns
        /// </summary>
        IGridHtmlOptions<T> Filterable(bool enable);

        /// <summary>
        /// Setup the text, which will displayed with empty items collection in the grid
        /// </summary>
        /// <param name="text">Grid empty text</param>
        IGridHtmlOptions<T> EmptyText(string text);

        /// <summary>
        /// Setup the language of Grid.Mvc
        /// </summary>
        /// <param name="lang">SetLanguage string (example: "en", "ru", "fr" etc.)</param>
        IGridHtmlOptions<T> SetLanguage(string lang);
    }
}