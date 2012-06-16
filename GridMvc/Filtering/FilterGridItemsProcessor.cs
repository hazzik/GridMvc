using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GridMvc.Columns;
using GridMvc.Utility;

namespace GridMvc.Filtering
{
    /// <summary>
    /// Grid items filter proprocessor
    /// </summary>
    internal class FilterGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private readonly IGridFilterSettings _settings;

        public FilterGridItemsProcessor(IGrid grid, IGridFilterSettings settings)
        {
            _grid = grid;
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            if (string.IsNullOrEmpty(_settings.ColumnName) || string.IsNullOrEmpty(_settings.Value))
                return items;
            IEnumerable<PropertyInfo> sequence;
            PropertyInfo pi = PropertiesHelper.GetPropertyFromColumnName(_settings.ColumnName, typeof (T), out sequence);
            if (pi == null) return items; // this property does not exist

            //determine gridColumn sortable:
            var gridColumn = _grid.Columns.FirstOrDefault(c => c.Name == _settings.ColumnName) as IGridColumn<T>;
            if (gridColumn == null || !gridColumn.FilterEnabled)
                return items;

            foreach (var columnFilter in gridColumn.Filters)
            {
                items = columnFilter.ApplyFilter(items, _settings);
            }
            return items;
        }

        #endregion
    }
}