using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GridMvc.Filtering;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    ///     Колонка, которая выводит содержимое свойства модели
    /// </summary>
    public class HiddenGridColumn<T, TDataType> : GridColumnBase<T>
    {
        private readonly Func<T, TDataType> _constraint;
        private readonly IGrid _grid;
        private IGridCellRenderer _cellRenderer;
        private readonly List<IColumnOrderer<T>> _orderers = new List<IColumnOrderer<T>>();


        public HiddenGridColumn(Expression<Func<T, TDataType>> expression, IGrid grid)
        {
            _grid = grid;
            _cellRenderer = new GridHiddenCellRenderer();
            SortEnabled = false;

            if (expression != null)
            {
                var expr = expression.Body as MemberExpression;
                if (expr == null)
                    throw new ArgumentException(
                        string.Format("Expression '{0}' must be a member expression", expression),
                        "expression");

                _constraint = expression.Compile();
                _orderers.Insert(0, new OrderByGridOrderer<T, TDataType>(expression));

                Name = PropertiesHelper.BuildColumnNameFromMemberExpression(expr);
            }
        }

        public override IEnumerable<IColumnOrderer<T>> Orderers
        {
            get { return _orderers; }
        }

        public override IGridColumnHeaderRenderer HeaderRenderer
        {
            get { return new GridHiddenHeaderRenderer(); }
            set { throw new InvalidOperationException("You can't set header renderer of hidden column"); }
        }

        public override IGridCellRenderer CellRenderer
        {
            get { return _cellRenderer; }
            set { _cellRenderer = value; }
        }

        public override bool FilterEnabled
        {
            get { return false; }
            set { }
        }

        public override IColumnFilter<T> Filter
        {
            get { return null; }
        }

        public override string FilterWidgetTypeName
        {
            get { return PropertiesHelper.GetUnderlyingType(typeof(TDataType)).FullName; }
        }

        //public override bool IsSorted { get; set; }
        //public override GridSortDirection? Direction { get; set; }

        public override IGrid ParentGrid
        {
            get { return _grid; }
        }

        public override IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData)
        {
            return this; //Do nothing
        }

        public override IGridColumn<T> SetFilterWidgetType(string typeName)
        {
            return this; //Do nothing
        }

        public override IGridColumn<T> SortInitialDirection(GridSortDirection direction)
        {
            if(string.IsNullOrEmpty(_grid.Settings.SortSettings.ColumnName)) {
                IsSorted = true;
                Direction = direction;
            }
            return this;
        }

        public override IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            _orderers.Add(new ThenByColumnOrderer<T, TKey>(expression, GridSortDirection.Ascending));
            return this;
        }

        public override IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression)
        {
            _orderers.Add(new ThenByColumnOrderer<T, TKey>(expression, GridSortDirection.Descending));
            return this;
        }

        public override IGridColumn<T> Sortable(bool sort)
        {
            if(sort && _constraint == null)
            {
                return this; //cannot enable sorting for column without expression
            }
            SortEnabled = sort;
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

                TDataType value = default(TDataType);
                var nullReferece = false;
                try
                {
                    value = _constraint(instance);
                }
                catch (NullReferenceException)
                {
                    nullReferece = true;
                    // specified expression throws NullReferenceException
                    // example: x=>x.Child.Property, when Child is NULL
                }

                if (nullReferece || value == null)
                    textValue = string.Empty;
                else if (!string.IsNullOrEmpty(ValuePattern))
                    textValue = string.Format(ValuePattern, value);
                else
                    textValue = value.ToString();
            }
            if (!EncodeEnabled && SanitizeEnabled)
            {
                textValue = _grid.Sanitizer.Sanitize(textValue);
            }
            return new GridCell(textValue) { Encode = EncodeEnabled };
        }

        public override IGridColumn<T> Filterable(bool showColumnValuesVariants)
        {
            return this;
        }

        public override IGridCell GetCell(object instance)
        {
            return GetValue((T)instance);
        }
    }
}