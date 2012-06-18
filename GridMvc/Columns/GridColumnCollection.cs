using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
    /// <summary>
    /// Collection of collumns
    /// </summary>
    public class GridColumnCollection<T> : KeyedCollection<string, IGridColumn>, IGridColumnCollection<T>
    {
        private readonly IColumnBuilder<T> _columnBuilder;
        private readonly IGridSettingsProvider _settingsProvider;

        public GridColumnCollection(IColumnBuilder<T> columnBuilder, IGridSettingsProvider settingsProvider)
        {
            _columnBuilder = columnBuilder;
            _settingsProvider = settingsProvider;
        }

        public bool DefaultSortEnabled { get; set; }
        public bool DefaultFilteringEnabled { get; set; }

        #region IGridColumnCollection<T> Members

        public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint)
        {
            return Add(constraint, false);
        }

        public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, bool hidden)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateColumn(constraint, hidden);
            return Add(newColumn);
        }

        public IGridColumn<T> Insert(int position, IGridColumn<T> column)
        {
            column.Sortable(DefaultSortEnabled);
            column.Filterable(DefaultFilteringEnabled);
            base.Insert(position, column);
            ProcessColumn();
            return column;
        }

        public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint)
        {
            return Insert(position, constraint, false);
        }

        public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, bool hidden)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateColumn(constraint, hidden);
            return Insert(position, newColumn);
        }

        public IGridColumn<T> Add(IGridColumn<T> column)
        {
            column.Sortable(DefaultSortEnabled);
            column.Filterable(DefaultFilteringEnabled);
            if (Contains(column))
                throw new ArgumentException("Column mapped to this field already exist in the grid");
            base.Add(column);
            ProcessColumn();
            return column;
        }

        public new IEnumerator<IGridColumn> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        #endregion

        private void ProcessColumn()
        {
            if (!string.IsNullOrEmpty(_settingsProvider.SortSettings.ColumnName))
            {
                foreach (IGridColumn gridColumn in this)
                {
                    gridColumn.IsSorted = gridColumn.Name == _settingsProvider.SortSettings.ColumnName;
                    if (gridColumn.Name == _settingsProvider.SortSettings.ColumnName)
                        gridColumn.Direction = _settingsProvider.SortSettings.Direction;
                    else
                        gridColumn.Direction = null;
                }
            }
        }

        protected override string GetKeyForItem(IGridColumn item)
        {
            return item.Name;
        }
    }
}