using System.Collections.Generic;
using GridMvc.Columns;
using GridMvc.Pagination;

namespace GridMvc
{
    public interface IGrid
    {
        /// <summary>
        /// Grid columns
        /// </summary>
        IGridColumnCollection Columns { get; }

        /// <summary>
        /// Grid items
        /// </summary>
        IEnumerable<object> ItemsToDisplay { get; }

        /// <summary>
        /// ItemsToDisplay count of the grid
        /// </summary>
        int ItemsCount { get; set; }

        /// <summary>
        /// Displaying items count
        /// </summary>
        int DisplayingItemsCount { get; }

        /// <summary>
        /// Pager for the grid
        /// </summary>
        IGridPager Pager { get; }

        /// <summary>
        /// Enable paging view
        /// </summary>
        bool EnablePaging { get; }

        /// <summary>
        /// Text in empty grid (no items for display)
        /// </summary>
        string EmptyGridText { get; }

        /// <summary>
        /// Returns the current Grid language
        /// </summary>
        string Language { get; }

        ISanitizer Sanitizer { get; }

        void OnPreRender();
    }
}