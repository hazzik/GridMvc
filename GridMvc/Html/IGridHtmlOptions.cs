using System.Web;

namespace GridMvc.Html
{
    /// <summary>
    /// Grid options for html helper
    /// </summary>
    public interface IGridHtmlOptions : IGrid, IHtmlString
    {
        /// <summary>
        /// Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <returns></returns>
        IGridHtmlOptions WithPaging(int pageSize);

        /// <summary>
        /// Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <param name="maxDisplayedItems">Setup max count of displaying pager links</param>
        /// <returns></returns>
        IGridHtmlOptions WithPaging(int pageSize, int maxDisplayedItems);

        /// <summary>
        /// Setup the text, which will displayed with empty items collection in the grid
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IGridHtmlOptions EmptyText(string text);
    }
}