using System.Globalization;
using GridMvc.Columns;

namespace GridMvc.Sorting
{
    /// <summary>
    /// Renderer for sortable column
    /// </summary>
    internal class QueryStringSortColumnHeaderRenderer : GridHeaderRenderer
    {
        private const string SortLinkContent = "<a href='{0}'>{1}</a>";
        private const string SortArrowContent = "<span class=\"grid-sort-arrow\"></span>";
        private readonly QueryStringSortSettings _settings;

        public QueryStringSortColumnHeaderRenderer(QueryStringSortSettings settings)
        {
            _settings = settings;
        }

        public override string Render(IGridColumn column, string content)
        {
            if (column.SortEnabled)
            {
                content = string.Format(SortLinkContent, GetLinkForSort(column.Name, column.Direction),
                                        content);
            }
            if (column.IsSorted)
            {
                AddCssClass("sorted");
                AddCssClass(column.Direction == GridSortDirection.Ascending ? "sorted-asc" : "sorted-desc");
                content += SortArrowContent;
            }

            return base.Render(column, content);
        }

        private string GetLinkForSort(string columnName, GridSortDirection? direction)
        {
            //switch direction for link:
            GridSortDirection newDir = direction == GridSortDirection.Ascending
                                           ? GridSortDirection.Descending
                                           : GridSortDirection.Ascending;
            return string.Format("?{0}={1}&{2}={3}", _settings.ColumnQueryParameterName, columnName,
                                 _settings.DirectionQueryParameterName,
                                 ((int) newDir).ToString(CultureInfo.InvariantCulture));
        }
    }
}