using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GridMvc.Sorting;

namespace GridMvc.Columns
{
    public interface IGridColumn<T> : ISortableColumn<T>, IGridColumn
    {
    }

    public interface IGridColumn : ISortableColumn
    {
    }

    public interface IColumn<T>
    {
        /// <summary>
        /// Set gridColumn title
        /// </summary>
        /// <param name="title">Title text</param>
        IGridColumn<T> Titled(string title);

        /// <summary>
        /// Need to encode the content of the gridColumn
        /// </summary>
        /// <param name="encode">Yes/No</param>
        IGridColumn<T> Encoded(bool encode);

        /// <summary>
        /// Sanitize column value from XSS attacks
        /// </summary>
        /// <param name="sanitize">If true values from this column will be sanitized</param>
        IGridColumn<T> Sanitized(bool sanitize);

        /// <summary>
        /// Sets the width of the column
        /// </summary>
        IGridColumn<T> Width(string width);

        /// <summary>
        /// Sets the width of the column in pizels
        /// </summary>
        IGridColumn<T> Width(int width);

        /// <summary>
        /// Setup the custom rendere for property
        /// </summary>
        IGridColumn<T> RenderValueAs(Func<T, string> constraint);
    }

    public interface IColumn
    {
        /// <summary>
        /// Columns title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Internal name of the gridColumn
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Width of the column
        /// </summary>
        string Width { get; }

        /// <summary>
        /// EncodeEnabled
        /// </summary>
        bool EncodeEnabled { get; }

        IGridColumnRenderer HeaderRenderer { get; }
        IGridColumnRenderer CellRenderer { get; }

        /// <summary>
        /// Gets value of the gridColumn by instance
        /// </summary>
        /// <param name="instance">Instance of the item</param>
        IGridCell GetCell(object instance);
    }

    public interface ISortableColumn<T> : IColumn<T>
    {
        /// <summary>
        /// List of column orderes
        /// </summary>
        IEnumerable<IColumnOrderer<T>> Orderers { get; }

        /// <summary>
        /// Enable sort of the gridColumn
        /// </summary>
        /// <param name="sort">Yes/No</param>
        IGridColumn<T> Sortable(bool sort);

        /// <summary>
        /// Setup the initial sorting direction of current column
        /// </summary>
        /// <param name="direction">Ascending / Descending</param>
        IGridColumn<T> SortInitialDirection(GridSortDirection direction);

        /// <summary>
        /// Setup ThenBy sorting of current column
        /// </summary>
        IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        /// Setup ThenByDescending sorting of current column
        /// </summary>
        IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression);
    }

    public interface ISortableColumn : IColumn
    {
        /// <summary>
        /// Internal name of the gridColumn
        /// </summary>
        bool SortEnabled { get; }

        /// <summary>
        /// Is current column sorted
        /// </summary>
        bool IsSorted { get; set; }

        /// <summary>
        ///Sort direction of current column
        /// </summary>
        GridSortDirection? Direction { get; set; }
    }
}