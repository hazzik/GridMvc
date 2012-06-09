using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
    public interface IGridColumnCollection<T> : IGridColumnCollection
    {
        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="column">Columns</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Add(IGridColumn<T> column);

        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="constraint">Member of generic class</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint);

        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="constraint">Member of generic class</param>
        /// <param name="hidden">Hidden column not display in grid, but you can get values from client side</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, bool hidden);

        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="position">Position to insert</param>
        /// <param name="column">Columns</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Insert(int position, IGridColumn<T> column);

        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="position">Position to insert</param>
        /// <param name="constraint">Member of generic class</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint);

        /// <summary>
        /// Add new column to the grid
        /// </summary>
        /// <param name="position">Position to insert</param>
        /// <param name="constraint">Member of generic class</param>
        /// <param name="hidden">Hidden column not display in grid, but you can get values from client side</param>
        /// <returns>Added column</returns>
        IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, bool hidden);
    }

    public interface IGridColumnCollection : IEnumerable<IGridColumn>
    {
    }
}