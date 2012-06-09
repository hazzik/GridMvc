using System;
using System.Web;

namespace GridMvc.Sorting
{
    /// <summary>
    /// Grid sort settings takes from query string
    /// </summary>
    public class QueryStringSortSettings : IGridSortSettings
    {
        public const string DefaultDirectionQueryParameter = "grid-dir";
        public const string DefaultColumnQueryParameter = "grid-column";
        private string _columnQueryParameterName;
        private string _directionQueryParameterName;

        public QueryStringSortSettings()
        {
            if (HttpContext.Current == null)
                throw new Exception("No http context here!");
            ColumnQueryParameterName = DefaultColumnQueryParameter;
            DirectionQueryParameterName = DefaultDirectionQueryParameter;
        }

        public QueryStringSortSettings(string columnName, GridSortDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
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

        public string DirectionQueryParameterName
        {
            get { return _directionQueryParameterName; }
            set
            {
                _directionQueryParameterName = value;
                RefreshDirection();
            }
        }

        #region IGridSortSettings Members

        public string ColumnName { get; set; }
        public GridSortDirection Direction { get; set; }

        #endregion

        private void RefreshColumn()
        {
            //Columns
            string currentSortColumn = HttpContext.Current.Request.QueryString[ColumnQueryParameterName] ?? string.Empty;
            if (string.IsNullOrEmpty(currentSortColumn))
            {
                ColumnName = currentSortColumn;
                Direction = GridSortDirection.Ascending;
                return;
            }
            ColumnName = currentSortColumn;
        }

        private void RefreshDirection()
        {
            //Direction
            string currentDirection = HttpContext.Current.Request.QueryString[DirectionQueryParameterName] ??
                                      string.Empty;
            if (string.IsNullOrEmpty(currentDirection))
            {
                Direction = GridSortDirection.Ascending;
                return;
            }
            GridSortDirection dir;
            Enum.TryParse(currentDirection, true, out dir);
            Direction = dir;
        }
    }
}