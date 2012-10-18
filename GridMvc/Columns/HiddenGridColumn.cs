using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GridMvc.Filtering;
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
        private IGridColumnRenderer _cellRenderer;

        public HiddenGridColumn(Expression<Func<T, TDataType>> expression, IGrid grid)
        {
            _grid = grid;
            if (expression!=null)
            {
                _constraint = expression.Compile();
                Name = PropertiesHelper.BuildColumnNameFromMemberExpression((MemberExpression)expression.Body);
            }
            
            _cellRenderer = new GridHiddenCellRenderer();
            
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
            get { return _cellRenderer; }
            set { _cellRenderer = value; }
        }

        public override bool FilterEnabled
        {
            get { return false; }
            set { throw new InvalidOperationException("You cannot filter hidden field"); }
        }

        public override bool IsFiltered
        {
            get { return false; }
            set { throw new InvalidOperationException("You cannot filter hidden field"); }
        }

        public override IEnumerable<IColumnFilter<T>> Filters
        {
            get { return Enumerable.Empty<IColumnFilter<T>>(); }
        }

        public override string FilterWidgetTypeName
        {
            get { return PropertiesHelper.GetUnderlyingType(typeof(TDataType)).FullName; }
        }

        public override IGridColumn<T> SetFilterWidgetType(string typeName)
        {
            throw new InvalidOperationException("You cannot filter hidden field");
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
                if (_constraint == null)
                {
                    throw new InvalidOperationException("You need to specify render expression using RenderValueAs");
                }
                TDataType value = _constraint(instance);
                if (value == null)
                    textValue = string.Empty;
                else if (!string.IsNullOrEmpty(ValuePattern))
                    textValue = string.Format(ValuePattern, value);
                else
                    textValue = value.ToString();
            }
            if (!EncodeEnabled && _sanitize)
            {
                textValue = _grid.Sanitizer.Sanitize(textValue);
            }
            return new GridCell(textValue) { Encode = EncodeEnabled };
        }

        public override IGridColumn<T> Filterable(bool showColumnValuesVariants)
        {
            throw new InvalidOperationException("You cannot filter hidden field");
        }

        public override IGridCell GetCell(object instance)
        {
            return GetValue((T)instance);
        }
    }
}