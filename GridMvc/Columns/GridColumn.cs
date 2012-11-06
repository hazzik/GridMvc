using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.DataAnnotations;
using GridMvc.Filtering;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    /// Default implementation of Grid column
    /// </summary>
    public class GridColumn<T, TDataType> : GridColumnBase<T> where T : class
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


        private IGridColumnRenderer _cellRenderer;
        private string _filterWidgetTypeName;

        public GridColumn(Expression<Func<T, TDataType>> expression, Grid<T> grid)
        {
            #region Setup defaults

            EncodeEnabled = true;
            SortEnabled = false;
            SanitizeEnabled = true;

            _filterWidgetTypeName = PropertiesHelper.GetUnderlyingType(typeof (TDataType)).FullName;
            _grid = grid;

            _cellRenderer = new GridCellRenderer();

            #endregion

            if (expression != null)
            {
                var expr = expression.Body as MemberExpression;
                if (expr == null)
                    throw new ArgumentException(
                        string.Format("Expression '{0}' must be a member expression", expression),
                        "expression");


                _constraint = expression.Compile();
                _orderers.Insert(0, new OrderByGridOrderer<T, TDataType>(expression));
                _filters.Insert(0, new DefaultColumnFilter<T, TDataType>(expression));
                //Generate unique column name:
                Name = PropertiesHelper.BuildColumnNameFromMemberExpression(expr);
                Title = Name; //Using the same name by default
                //Apply data annotation options
                var pi = expr.Member as PropertyInfo;
                if (pi != null)
                    ApplyColumnSettings(pi);
            }
        }

        public override IGridColumnRenderer HeaderRenderer
        {
            get { return _grid.Settings.HeaderRenderer; }
        }

        public override IGridColumnRenderer CellRenderer
        {
            get { return _cellRenderer; }
            set { _cellRenderer = value; }
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
            if (sort && _constraint == null)
            {
                return this; //cannot enable sorting for column without expression
            }
            SortEnabled = sort;
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
            if (!EncodeEnabled && SanitizeEnabled)
            {
                textValue = _grid.Sanitizer.Sanitize(textValue);
            }
            return new GridCell(textValue) {Encode = EncodeEnabled};
        }

        public override IGridColumn<T> Filterable(bool enable)
        {
            if (enable && _constraint == null)
            {
                return this; //cannot enable filtering for column without expression
            }
            FilterEnabled = enable;
            return this;
        }

        private void ApplyColumnSettings(PropertyInfo pi)
        {
            var options = pi.GetAttribute<GridColumnAttribute>();
            if (options == null) return;

            Encoded(options.EncodeEnabled)
                .Sanitized(options.SanitizeEnabled)
                .Filterable(options.FilterEnabled)
                .Sortable(options.SortEnabled);

            GridSortDirection? initialDirection = options.GetInitialSortDirection();
            if (initialDirection.HasValue)
                SortInitialDirection(initialDirection.Value);

            if (!string.IsNullOrEmpty(options.FilterWidgetType))
                SetFilterWidgetType(options.FilterWidgetType);

            if (!string.IsNullOrEmpty(options.Format))
                Format(options.Format);
            if (!string.IsNullOrEmpty(options.Title))
                Titled(options.Title);
            if (!string.IsNullOrEmpty(options.Width))
                Width = options.Width;
        }
    }
}