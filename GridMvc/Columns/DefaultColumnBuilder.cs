using System;
using System.Linq.Expressions;

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
            var memberExpression = constraint.Body as MemberExpression;
            if (memberExpression != null)
            {
                if (!hidden)
                    return new DefaultGridColumn<T, TDataType>(constraint, _grid);
                return new HiddenGridColumn<T, TDataType>(constraint, _grid);
            }
            throw new NotSupportedException(string.Format("Expression '{0}' not supported by grid", constraint));
        }

        #endregion
    }
}