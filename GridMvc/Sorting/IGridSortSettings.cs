namespace GridMvc.Sorting
{
    /// <summary>
    /// Settings for sort
    /// </summary>
    public interface IGridSortSettings
    {
        /// <summary>
        /// Column name for sort
        /// </summary>
        string ColumnName { get; set; }

        /// <summary>
        /// Direction of sorting
        /// </summary>
        GridSortDirection Direction { get; set; }
    }
}