using System;
using System.Linq;
using System.Web;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Object gets filter settings from query string
    /// </summary>
    public class QueryStringFilterSettings : IGridFilterSettings
    {
        public const string DefaultTypeQueryParameter = "grid-filter";
        private const string FilterDataDelimeter = "__";
        private const string DefaultFilterInitQueryParameter = "grid-init";
        public readonly HttpContext Context;
        private readonly DefaultFilterColumnCollection _columns = new DefaultFilterColumnCollection();

        #region Ctor's

        public QueryStringFilterSettings()
            : this(HttpContext.Current)
        {
            
        }

        public QueryStringFilterSettings(HttpContext context)
        {
            if (context == null)
                throw new ArgumentException("No http context here!");
            Context = context;

            var filters = Context.Request.QueryString.GetValues(DefaultTypeQueryParameter);
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var column = CreateColumnData(filter);
                    if (column != ColumnFilterValue.Null)
                        _columns.Add(column);
                }

            }
        }

        #endregion

        private ColumnFilterValue CreateColumnData(string queryParameterValue)
        {
            if (string.IsNullOrEmpty(queryParameterValue))
                return ColumnFilterValue.Null;

            var data = queryParameterValue.Split(new[] { FilterDataDelimeter }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != 3)
                return ColumnFilterValue.Null;
            GridFilterType type;
            if (!Enum.TryParse(data[1], true, out type))
                type = GridFilterType.Equals;

            return new ColumnFilterValue { ColumnName = data[0], FilterType = type, FilterValue = data[2] };
        }


        #region IGridFilterSettings Members

        public IFilterColumnCollection FilteredColumns
        {
            get { return _columns; }
        }

        public bool IsInitState
        {
            get
            {
                if (FilteredColumns.Any()) return false;
                return Context.Request.QueryString[DefaultFilterInitQueryParameter] != null;
            }
        }

        #endregion

    }
}