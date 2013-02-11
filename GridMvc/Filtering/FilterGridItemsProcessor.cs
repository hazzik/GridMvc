using System;
using System.Linq;
using GridMvc.Columns;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Grid items filter proprocessor
    /// </summary>
    internal class FilterGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private IGridFilterSettings _settings;

        public FilterGridItemsProcessor(IGrid grid, IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _grid = grid;
            _settings = settings;
        }

        public void UpdateSettings(IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            if (_settings.IsInitState)
            {
                //filter not set
                foreach (IGridColumn column in _grid.Columns)
                {
                    var gridColumn = column as IGridColumn<T>;
                    if (gridColumn == null) continue;
                    if (gridColumn.InitialFilterSettings != null)
                        foreach (var columnFilter in gridColumn.Filters)
                        {
                            items = columnFilter.ApplyFilter(items, gridColumn.InitialFilterSettings);
                        }
                }
                return items;
            }

            //determine gridColumn sortable:
            var filterByColumn = _grid.Columns.FirstOrDefault(c => c.Name == _settings.ColumnName) as IGridColumn<T>;
            if (filterByColumn == null || !filterByColumn.FilterEnabled)
                return items;

            foreach (var columnFilter in filterByColumn.Filters)
            {
                items = columnFilter.ApplyFilter(items, _settings);
            }
            return items;
        }

        #endregion
    }
}