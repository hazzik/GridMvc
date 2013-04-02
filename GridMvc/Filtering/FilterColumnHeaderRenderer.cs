using System.Collections.Generic;
using System.Linq;
using GridMvc.Columns;
using GridMvc.Resources;
using GridMvc.Utility;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Renderer for sortable column
    /// </summary>
    internal class QueryStringFilterColumnHeaderRenderer : IGridColumnRenderer
    {
        private const string FilteredButtonCssClass = "filtered";
        private const string FilterContent =
            @" <span 
                                            data-type='{1}'
                                            data-name='{2}'
                                            data-filterdata='{3}'
                                            data-url='{4}'
                                            class='grid-filter'>
                                        <span class='grid-filter-btn {5}' title='{0}'></span>
                                    </span>";

        private readonly QueryStringFilterSettings _settings;

        public QueryStringFilterColumnHeaderRenderer(QueryStringFilterSettings settings)
        {
            _settings = settings;
        }

        #region IGridColumnRenderer Members

        public string Render(IGridColumn column, string content)
        {
            if (!column.FilterEnabled)
                return string.Empty;

            var filterSettings = _settings.IsInitState
                                                     ? new List<ColumnFilterValue> { column.InitialFilterSettings }
                                                     : _settings.FilteredColumns.GetByColumn(column).ToList();

            bool isColumnFiltered = filterSettings.Any();

            //determine current url:
            var builder = new CustomQueryStringBuilder(_settings.Context.Request.QueryString);
            string url =
                builder.GetQueryStringExcept(new[]
                    {
                        column.ParentGrid.Pager.ParameterName,
                        QueryStringFilterSettings.DefaultTypeQueryParameter,
                        //_settings.TypeQueryParameterName,
                        //_settings.ColumnQueryParameterName,
                        //_settings.ValueQueryParameterName
                    });

            return string.Format(FilterContent,
                                 Strings.FilterButtonTooltipText,
                                 column.FilterWidgetTypeName,
                                 column.Name,
                                 JsonHelper.JsonSerializer(filterSettings),
                                 url,
                                 isColumnFiltered ? FilteredButtonCssClass : string.Empty);
        }

        #endregion
    }
}