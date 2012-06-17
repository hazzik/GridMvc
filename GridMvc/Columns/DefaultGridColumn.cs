using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GridMvc.Filtering;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    /// Default implementation of Grid column
    /// </summary>
    public class DefaultGridColumn<T, TDataType> : GridColumnBase<T> where T : class
    {
        /// <summary>
        /// Expression to member, used for this column
        /// </summary>
        private readonly Func<T, TDataType> _constraint;

        /// <summary>
        /// Filters and orderers collection for this columns
        /// </summary>
        private readonly List<IColumnFilter<T>> _filters = new List<IColumnFilter<T>>();

        /// <summary>
        /// Parent grid of this column
        /// </summary>
        private readonly Grid<T> _grid;

        private readonly List<IColumnOrderer<T>> _orderers = new List<IColumnOrderer<T>>();

        private string _filterWidgetTypeName;
        private bool _sanitize;

        public DefaultGridColumn(Expression<Func<T, TDataType>> expression, Grid<T> grid)
        {
            Expression expr = expression.Body;
            if (!(expr is MemberExpression))
                throw new ArgumentException(string.Format("Expression '{0}' must be a member expression", expression),
                                            "expression");

            #region Setup defaults

            EncodeEnabled = true;
            SortEnabled = false;
            _constraint = expression.Compile();
            _orderers.Insert(0, new OrderByGridOrderer<T, TDataType>(expression));
            _filters.Insert(0, new DefaultColumnFilter<T, TDataType>(expression));
            _filterWidgetTypeName = PropertiesHelper.GetUnderlyingType(typeof (TDataType)).FullName;
            _grid = grid;
            _sanitize = true;

            #endregion

            //Generate unique column name:
            Name = PropertiesHelper.BuildColumnNameFromMemberExpression((MemberExpression) expression.Body);
            Title = Name; //Useing the same name by default
        }

        public override IGridColumnRenderer HeaderRenderer
        {
            get { return _grid.Settings.HeaderRenderer; }
        }

        public override IGridColumnRenderer CellRenderer
        {
            get { return new GridCellRenderer(); }
        }

        public override IEnumerable<IColumnOrderer<T>> Orderers
        {
            get { return _orderers; }
        }

        public override bool FilterEnabled { get; set; }
        public override bool IsFiltered { get; set; }

        public override IEnumerable<IColumnFilter<T>> Filters
        {
            get { return _filters; }
        }

        public override string FilterWidgetTypeName
        {
            get { return _filterWidgetTypeName; }
        }

        public override IGridColumn<T> SetFilterWidgetType(string typeName)
        {
            _filterWidgetTypeName = typeName;
            return this;
        }

        public override IGridColumn<T> SortInitialDirection(GridSortDirection direction)
        {
            IsSorted = true;
            Direction = direction;
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
            SortEnabled = sort;
            return this;
        }

        public override IGridColumn<T> Sanitized(bool sanitize)
        {
            _sanitize = sanitize;
            return this;
        }

        public override IGridCell GetCell(object instance)
        {
            return GetValue((T) instance);
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

        public override IGridColumn<T> Filterable(bool showColumnValuesVariants)
        {
            FilterEnabled = true;
            return this;
        }
    }
}