using System;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
    public interface IColumnBuilder<T>
    {
        IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> expression, bool hidden);
    }
}