using System;
using System.Web;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Object gets filter settings from query string
    /// </summary>
    public class QueryStringFilterSettings : IGridFilterSettings
    {
        private const string DefaultTypeQueryParameter = "grid-filter-type";
        private const string DefaultColumnQueryParameter = "grid-filter-col";
        private const string DefaultValueQueryParameter = "grid-filter-val";
        private const string DefaultFilterInitQueryParameter = "grid-init";


        public readonly HttpContext Context;

        private string _columnQueryParameterName;
        private string _filterInitQueryParameterName;
        private string _typeQueryParameterName;
        private string _valueQueryParameterName;

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

            Type = GridFilterType.Equals;

            ColumnQueryParameterName = DefaultColumnQueryParameter;
            ValueQueryParameterName = DefaultValueQueryParameter;
            TypeQueryParameterName = DefaultTypeQueryParameter;

            _filterInitQueryParameterName = DefaultFilterInitQueryParameter;
        }

        #endregion

        public virtual string FilterInitQueryParameterName
        {
            get { return _filterInitQueryParameterName; }
            set { _filterInitQueryParameterName = value; }
        }

        public string ColumnQueryParameterName
        {
            get { return _columnQueryParameterName; }
            set
            {
                _columnQueryParameterName = value;
                RefreshColumn();
            }
        }

        public string ValueQueryParameterName
        {
            get { return _valueQueryParameterName; }
            set
            {
                _valueQueryParameterName = value;
                RefreshValue();
            }
        }

        public string TypeQueryParameterName
        {
            get { return _typeQueryParameterName; }
            set
            {
                _typeQueryParameterName = value;
                RefreshType();
            }
        }

        #region IGridFilterSettings Members

        public string ColumnName { get; set; }
        public string Value { get; set; }
        public GridFilterType Type { get; set; }

        public bool IsInitState
        {
            get
            {
                bool isEmptyValues = string.IsNullOrEmpty(ColumnName) || string.IsNullOrEmpty(Value);
                if (!isEmptyValues) return false;
                return Context.Request.QueryString[FilterInitQueryParameterName] != null;
            }
        }

        #endregion

        private void RefreshColumn()
        {
            string currentFilterColumn = Context.Request.QueryString[ColumnQueryParameterName] ?? string.Empty;
            ColumnName = currentFilterColumn;
        }

        private void RefreshValue()
        {
            //string currentFilterValue = HttpUtility.ParseQueryString(Context.Request.Url.Query, Encoding.GetEncoding("iso-8859-1"))[ValueQueryParameterName];
            string currentFilterValue = Context.Request.QueryString[ValueQueryParameterName] ?? string.Empty;
            Value = currentFilterValue;
        }

        private void RefreshType()
        {
            string currentFilterType = Context.Request.QueryString[TypeQueryParameterName] ?? string.Empty;
            if (string.IsNullOrEmpty(currentFilterType))
            {
                Type = GridFilterType.Equals;
                return;
            }
            GridFilterType type;
            Enum.TryParse(currentFilterType, true, out type);
            Type = type;
        }
    }
}