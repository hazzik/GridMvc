using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using GridMvc.DataAnnotations;
using GridMvc.Sorting;

namespace GridMvc.Columns
{
    /// <summary>
    ///     Default grid columns builder. Creates the columns from expression
    /// </summary>
    internal class DefaultColumnBuilder<T> : IColumnBuilder<T> where T : class
    {
        private readonly IGridAnnotaionsProvider _annotaions;
        private readonly Grid<T> _grid;

        public DefaultColumnBuilder(Grid<T> grid, IGridAnnotaionsProvider annotaions)
        {
            _grid = grid;
            _annotaions = annotaions;
        }

        #region IColumnBuilder<T> Members

        public IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> constraint, bool hidden)
        {
            bool isExpressionOk = constraint == null || constraint.Body as MemberExpression != null;
            if (isExpressionOk)
            {
                if (!hidden)
                    return new GridColumn<T, TDataType>(constraint, _grid);
                return new HiddenGridColumn<T, TDataType>(constraint, _grid);
            }
            throw new NotSupportedException(string.Format("Expression '{0}' not supported by grid", constraint));
        }

        /// <summary>
        ///     Creates column from property info using reflection
        /// </summary>
        public IGridColumn<T> CreateColumn(PropertyInfo pi)
        {
            if (!_annotaions.IsColumnMapped(pi))
                return null; //grid column not mapped

            var metadata = ModelMetadata.FromStringExpression(pi.Name, new ViewDataDictionary<T>());
            var column = CreateColumn(pi, metadata.GetAdditionalValue<bool>(AdditionalMetadataKeys.HiddenKey));

            column.Titled(metadata.DisplayName)
                .Encoded(metadata.GetAdditionalValue<bool>(AdditionalMetadataKeys.EncodeEnabledKey))
                .Sanitized(metadata.GetAdditionalValue<bool>(AdditionalMetadataKeys.SanitizeEnabledKey))
                .Filterable(metadata.GetAdditionalValue<bool>(AdditionalMetadataKeys.FilterEnabledKey))
                .Sortable(metadata.GetAdditionalValue<bool>(AdditionalMetadataKeys.SortEnabledKey))
                .SortInitialDirection(metadata.GetAdditionalValue<GridSortDirection>(AdditionalMetadataKeys.SortInitialDirectionKey));

            if (!string.IsNullOrEmpty(metadata.GetAdditionalValue<string>(AdditionalMetadataKeys.FilterWidgetTypeKey)))
                column.SetFilterWidgetType(metadata.GetAdditionalValue<string>(AdditionalMetadataKeys.FilterWidgetTypeKey));

            if (!string.IsNullOrEmpty(metadata.DisplayFormatString))
                column.Format(metadata.DisplayFormatString);

            if (!string.IsNullOrEmpty(metadata.GetAdditionalValue<string>(AdditionalMetadataKeys.WidthKey)))
                column.Width = metadata.GetAdditionalValue<string>(AdditionalMetadataKeys.WidthKey);
            
            return column;
        }

        public bool DefaultSortEnabled { get; set; }
        public bool DefaultFilteringEnabled { get; set; }

        #endregion

        private IGridColumn<T> CreateColumn(PropertyInfo pi, bool hidden)
        {
            Type entityType = typeof (T);
            Type columnType;

            if (!hidden)
                columnType = typeof (GridColumn<,>).MakeGenericType(entityType, pi.PropertyType);
            else
                columnType = typeof (HiddenGridColumn<,>).MakeGenericType(entityType, pi.PropertyType);

            //Build expression

            ParameterExpression parameter = Expression.Parameter(entityType, "e");
            MemberExpression expressionProperty = Expression.Property(parameter, pi);

            Type funcType = typeof (Func<,>).MakeGenericType(entityType, pi.PropertyType);
            LambdaExpression lambda = Expression.Lambda(funcType, expressionProperty, parameter);

            var column = Activator.CreateInstance(columnType, lambda, _grid) as IGridColumn<T>;
            if (!hidden && column != null)
            {
                column.Sortable(DefaultSortEnabled);
                column.Filterable(DefaultFilteringEnabled);
            }
            return column;
        }
    }
}