using System;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
    /// <summary>
    /// Object which creates grid columns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColumnBuilder<T>
    {
        IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> expression, bool hidden);
    }
}