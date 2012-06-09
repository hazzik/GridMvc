using System;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
    public interface IColumnBuilder
    {
        IGridColumn<T> CreateColumn<T, TDataType>(Expression<Func<T, TDataType>> expression, bool hidden);
    }
}