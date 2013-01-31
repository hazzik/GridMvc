using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridMvc.Filtering
{
    /// <summary>
    /// Custom filter settings for filtering column
    /// </summary>
    public class CustomFilterSettings : IGridFilterSettings
    {
        private string _columnName;

        /// <summary>
        /// Create custom filter settings
        /// </summary>
        /// <param name="type">Filter type</param>
        /// <param name="value">Filter value</param>
        public CustomFilterSettings(GridFilterType type, string value)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }
        public GridFilterType Type { get; set; }
        public bool IsEmpty { get { return false; } }

        string IGridFilterSettings.ColumnName
        {
            get { return _columnName; }
        }

        public void SetColumnName(string columnName)
        {
            _columnName = columnName;
        }
    }
}
