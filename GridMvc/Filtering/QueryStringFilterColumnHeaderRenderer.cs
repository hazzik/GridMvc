using GridMvc.Columns;
using GridMvc.Pagination;
using GridMvc.Resources;
using GridMvc.Utility;

namespace GridMvc.Filtering
{
    /// <summary>
    /// Renderer for sortable column
    /// </summary>
    internal class QueryStringFilterColumnHeaderRenderer : IGridColumnRenderer
    {
        private const string FilteredButtonCssClass = "filtered";

        private const string FilterContent =
            @" <span title=""{0}"" 
                                            data-type=""{1}"" 
                                            data-name=""{2}""
                                            data-filtertype=""{3}""
                                            data-filtervalue=""{4}""
                                            data-url=""{5}""
                                            class=""grid-filter-btn {6}"">
                                    </span>";

        private readonly QueryStringFilterSettings _settings;

        public QueryStringFilterColumnHeaderRenderer(QueryStringFilterSettings settings)
        {
            _settings = settings;
        }

        #region IGridColumnRenderer Members

        public string Render(IGridColumn column, string content)
        {
            GridFilterType type = GridFilterType.Equals;
            string value = string.Empty;
            bool isColumnFiltered = false;
            if (column.Name == _settings.ColumnName)
            {
                //filter on this column:
                type = _settings.Type;
                value = _settings.Value;
                isColumnFiltered = true;
            }
            //determine current url:
            var builder = new CustomQueryStringBuilder(_settings.Context.Request.QueryString);
            string url =
                builder.GetQueryStringExcept(new[]
                                                 {
                                                     GridPager.DefaultPageQueryParameter,
                                                     _settings.TypeQueryParameterName,
                                                     _settings.ColumnQueryParameterName,
                                                     _settings.ValueQueryParameterName
                                                 });
            if (column.FilterEnabled)
                return string.Format(FilterContent,
                                     Strings.FilterButtonTooltipText,
                                     column.FilterWidgetTypeName,
                                     column.Name,
                                     (int) type,
                                     value,
                                     url,
                                     isColumnFiltered ? FilteredButtonCssClass : string.Empty);
            return string.Empty;
        }

        #endregion
    }
}