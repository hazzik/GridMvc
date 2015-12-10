using System;
using System.Linq;
using GridMvc.Columns;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Settings grid items, based on current sorting settings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SortGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private IGridSortSettings _settings;

        public SortGridItemsProcessor(IGrid grid, IGridSortSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _grid = grid;
            _settings = settings;
        }

        public void UpdateSettings(IGridSortSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            //if (string.IsNullOrEmpty(_settings.ColumnName))
            // UpdateColumnsSorting fix it
            //if (_grid.EnablePaging)//paging on EF require orderBy
            var sortColumn = _grid.Columns.FirstOrDefault(c => c.IsSorted) as IGridColumn<T>;
            if(sortColumn == null)
                return items;
            foreach (var columnOrderer in sortColumn.Orderers)
            {
                items = columnOrderer.ApplyOrder(items, sortColumn.Direction ?? GridSortDirection.Ascending);
            }
            return items;
        }

        #endregion
    }
}