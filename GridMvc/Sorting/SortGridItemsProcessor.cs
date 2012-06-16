using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GridMvc.Columns;
using GridMvc.Utility;

namespace GridMvc.Sorting
{
    /// <summary>
    /// Settings grid items, based on current sorting settings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SortGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private readonly IGridSortSettings _settings;

        public SortGridItemsProcessor(IGrid grid, IGridSortSettings settings)
        {
            _grid = grid;
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            if (string.IsNullOrEmpty(_settings.ColumnName))
                return items;
            IEnumerable<PropertyInfo> sequence;
            PropertyInfo pi = PropertiesHelper.GetPropertyFromColumnName(_settings.ColumnName, typeof (T), out sequence);
            if (pi == null) return items; // this property does not exist

            //determine gridColumn sortable:
            var gridColumn = _grid.Columns.FirstOrDefault(c => c.Name == _settings.ColumnName) as IGridColumn<T>;
            if (gridColumn == null || !gridColumn.SortEnabled)
                return items;
            foreach (var columnOrderer in gridColumn.Orderers)
            {
                items = columnOrderer.ApplyOrder(items, _settings.Direction);
            }
            return items;
        }

        #endregion
    }
}