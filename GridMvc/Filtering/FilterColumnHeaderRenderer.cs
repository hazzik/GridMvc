using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridMvc.Columns;
using GridMvc.Pagination;
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

        public IHtmlString Render(IGridColumn column, string content)
        {
            if (!column.FilterEnabled)
                return MvcHtmlString.Empty;

            //determine current column filter settings
            var filterSettings = new List<ColumnFilterValue>();
            if (_settings.IsInitState && column.InitialFilterSettings != ColumnFilterValue.Null)
            {
                filterSettings.Add(column.InitialFilterSettings);
            }
            else
            {
                filterSettings.AddRange(_settings.FilteredColumns.GetByColumn(column));
            }

            bool isColumnFiltered = filterSettings.Any();

            //determine current url:
            var builder = new CustomQueryStringBuilder(_settings.Context.Request.QueryString);

            var exceptQueryParameters = new List<string> { QueryStringFilterSettings.DefaultTypeQueryParameter, QueryStringFilterSettings.DefaultFilterInitQueryParameter };
            string pagerParameterName = GetPagerQueryParameterName(column.ParentGrid.Pager);
            if (!string.IsNullOrEmpty(pagerParameterName))
                exceptQueryParameters.Add(pagerParameterName);

            string url = builder.GetQueryStringExcept(exceptQueryParameters);

            return MvcHtmlString.Create(string.Format(FilterContent,
                                                      Strings.FilterButtonTooltipText,
                                                      column.FilterWidgetTypeName,
                                                      column.Name,
                                                      JsonHelper.JsonSerializer(filterSettings),
                                                      url,
                                                      isColumnFiltered ? FilteredButtonCssClass : string.Empty));
        }

        #endregion

        /// <summary>
        ///     Extract query string parameter name from default grid pager (if using)
        /// </summary>
        private string GetPagerQueryParameterName(IGridPager pager)
        {
            var defaultPager = pager as GridPager;
            if (defaultPager == null)
                return string.Empty;
            return defaultPager.ParameterName;
        }
    }
}