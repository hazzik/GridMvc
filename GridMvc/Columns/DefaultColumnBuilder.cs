using System;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.DataAnnotations;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    /// Default grid columns builder. Creates the columns from expression
    /// </summary>
    internal class DefaultColumnBuilder<T> : IColumnBuilder<T> where T : class
    {
        private readonly Grid<T> _grid;

        public DefaultColumnBuilder(Grid<T> grid)
        {
            _grid = grid;
        }

        #region IColumnBuilder<T> Members

        public IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> constraint, bool hidden)
        {
            var isExpressionOk = constraint == null || constraint.Body as MemberExpression != null;
            if (isExpressionOk)
            {
                if (!hidden)
                    return new GridColumn<T, TDataType>(constraint, _grid);
                return new HiddenGridColumn<T, TDataType>(constraint, _grid);
            }
            throw new NotSupportedException(string.Format("Expression '{0}' not supported by grid", constraint));
        }

        /// <summary>
        /// Creates column from property info using reflection
        /// </summary>
        public IGridColumn<T> CreateColumn(PropertyInfo pi)
        {
            if (pi.GetAttribute<NotMappedColumnAttribute>() != null)
                return null;
            IGridColumn<T> column;
            var gridOpt = pi.GetAttribute<GridColumnAttribute>();
            if (gridOpt != null)
            {
                column = CreateColumn(pi, false);
            }
            else
            {
                var gridHiddenOpt = pi.GetAttribute<GridHiddenColumnAttribute>();
                if (gridHiddenOpt != null)
                {
                    column = CreateColumn(pi, true);
                }
                else
                {
                    column = CreateColumn(pi, false);
                }
            }
            return column;
        }

        private IGridColumn<T> CreateColumn(PropertyInfo pi, bool hidden)
        {
            var entityType = typeof(T);
            Type columnType;

            if (!hidden)
                columnType = typeof(GridColumn<,>).MakeGenericType(entityType, pi.PropertyType);
            else
                columnType = typeof(HiddenGridColumn<,>).MakeGenericType(entityType, pi.PropertyType);

            //Build expression

            var parameter = Expression.Parameter(entityType, "e");
            var expressionProperty = Expression.Property(parameter, pi);

            var funcType = typeof(Func<,>).MakeGenericType(entityType, pi.PropertyType);
            var lambda = Expression.Lambda(funcType, expressionProperty, parameter);

            return Activator.CreateInstance(columnType, lambda, _grid) as IGridColumn<T>;
        }


        #endregion
    }
}