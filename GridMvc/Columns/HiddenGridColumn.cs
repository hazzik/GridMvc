using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    /// Колонка, которая выводит содержимое свойства модели
    /// </summary>
    public class HiddenGridColumn<T, TDataType> : GridColumnBase<T>
    {
        private readonly Func<T, TDataType> _constraint;
        private readonly IGrid _grid;
        private bool _sanitize;

        public HiddenGridColumn(Expression<Func<T, TDataType>> expression, IGrid grid)
        {
            _grid = grid;
            _constraint = expression.Compile();
            Name = PropertiesHelper.BuildColumnNameFromMemberExpression((MemberExpression) expression.Body);
            SortEnabled = false;
        }

        public override IEnumerable<IColumnOrderer<T>> Orderers
        {
            get { throw new InvalidOperationException("You cannot sort hidden field"); }
        }

        public override IGridColumnRenderer HeaderRenderer
        {
            get { return new GridHiddenHeaderRenderer(); }
        }

        public override IGridColumnRenderer CellRenderer
        {
            get { return new GridHiddenCellRenderer(); }
        }

        public override IGridColumn<T> SortInitialDirection(GridSortDirection direction)
        {
            throw new InvalidOperationException("You cannot setup initial sorting of hidden column");
        }

        public override IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            throw new InvalidOperationException("You cannot sort hidden field");
        }

        public override IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression)
        {
            throw new InvalidOperationException("You cannot sort hidden field");
        }

        public override IGridColumn<T> Sortable(bool sort)
        {
            throw new InvalidOperationException("You cannot sort hidden field");
        }

        public override IGridColumn<T> Sanitized(bool sanitize)
        {
            _sanitize = sanitize;
            return this;
        }

        public override IGridCell GetValue(T instance)
        {
            string textValue;
            if (ValueConstraint != null)
            {
                textValue = ValueConstraint(instance);
            }
            else
            {
                TDataType value = _constraint(instance);
                textValue = value == null ? string.Empty : value.ToString();
            }
            if (!EncodeEnabled && _sanitize)
            {
                textValue = _grid.Sanitizer.Sanitize(textValue);
            }
            return new GridCell(textValue) {Encode = EncodeEnabled};
        }

        public override IGridCell GetCell(object instance)
        {
            return GetValue((T) instance);
        }
    }
}