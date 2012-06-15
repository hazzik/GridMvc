using System.Globalization;
using System.Web;
using GridMvc.Columns;
using GridMvc.Pagination;
using GridMvc.Utility;

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
            return base.Render(column, GetSortHeaderContent(column, content));
        }

        protected string GetSortHeaderContent(IGridColumn column, string content)
        {
            if (column.SortEnabled)
            {
                var url = GetLinkForSort(column.Name, column.Direction);
                content = string.Format(SortLinkContent, url,
                                        content);
            }
            if (column.IsSorted)
            {
                AddCssClass("sorted");
                AddCssClass(column.Direction == GridSortDirection.Ascending ? "sorted-asc" : "sorted-desc");
                content += SortArrowContent;
            }
            return content;
        }

        private string GetLinkForSort(string columnName, GridSortDirection? direction)
        {
            //switch direction for link:
            GridSortDirection newDir = direction == GridSortDirection.Ascending
                                           ? GridSortDirection.Descending
                                           : GridSortDirection.Ascending;
            //determine current url:
            var builder = new CustomQueryStringBuilder(_settings.Context.Request.QueryString);
            string url =
                builder.GetQueryStringExcept(new[]
                                                 {
                                                     GridPager.DefaultPageQueryParameter,
                                                     _settings.ColumnQueryParameterName,
                                                     _settings.DirectionQueryParameterName
                                                 });
            if (string.IsNullOrEmpty(url))
                url = "?";
            else
                url += "&";
            return string.Format("{0}{1}={2}&{3}={4}", url, _settings.ColumnQueryParameterName, columnName,
                                 _settings.DirectionQueryParameterName,
                                 ((int) newDir).ToString(CultureInfo.InvariantCulture));
        }
    }
}