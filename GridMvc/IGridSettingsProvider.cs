using GridMvc.Sorting;

namespace GridMvc
{
    /// <summary>
    /// Setting for grid
    /// </summary>
    public interface IGridSettingsProvider
    {
        GridHeaderRenderer HeaderRenderer { get; }
        IGridSortSettings SortSettings { get; }
    }
}