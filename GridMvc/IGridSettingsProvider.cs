using GridMvc.Filtering;
using GridMvc.Sorting;

namespace GridMvc
{
    /// <summary>
    /// Settings for grid
    /// </summary>
    public interface IGridSettingsProvider
    {
        GridHeaderRenderer HeaderRenderer { get; }
        IGridSortSettings SortSettings { get; }
        IGridFilterSettings FilterSettings { get; }
    }
}