namespace GridMvc.Sorting
{
    /// <summary>
    /// Provider for grid sorting
    /// </summary>
    public interface IGridSortProvider
    {
        IGridColumnRenderer HeaderRenderer { get; }
        IGridSortSettings Settings { get; }
    }
}