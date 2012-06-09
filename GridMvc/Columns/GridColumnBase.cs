using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using GridMvc.Sorting;

namespace GridMvc.Columns
{
    public abstract class GridColumnBase<T> : IGridColumn<T>
    {
        protected Func<T, string> ValueConstraint;

        #region IGridColumn<T> Members

        public bool EncodeEnabled { get; protected set; }

        public string Width { get; protected set; }

        public bool SortEnabled { get; protected set; }

        public string Title { get; set; }
        public string Name { get; set; }

        public bool IsSorted { get; set; }
        public GridSortDirection? Direction { get; set; }

        public IGridColumn<T> Titled(string title)
        {
            Title = title;
            return this;
        }

        public IGridColumn<T> Encoded(bool encode)
        {
            EncodeEnabled = encode;
            return this;
        }

        IGridColumn<T> IColumn<T>.Width(string width)
        {
            Width = width;
            return this;
        }

        IGridColumn<T> IColumn<T>.Width(int width)
        {
            Width = width.ToString(CultureInfo.InvariantCulture) + "px";
            return this;
        }


        public IGridColumn<T> RenderValueAs(Func<T, string> constraint)
        {
            ValueConstraint = constraint;
            return this;
        }

        public abstract IGridColumn<T> SortInitialDirection(GridSortDirection direction);

        public abstract IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression);
        public abstract IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression);

        public abstract IEnumerable<IColumnOrderer<T>> Orderers { get; }
        public abstract IGridColumn<T> Sortable(bool sort);
        public abstract IGridColumn<T> Sanitized(bool sanitize);

        public abstract IGridColumnRenderer HeaderRenderer { get; }
        public abstract IGridColumnRenderer CellRenderer { get; }
        public abstract IGridCell GetCell(object instance);

        #endregion

        public abstract IGridCell GetValue(T instance);
    }
}