using GridMvc.Sorting;

namespace GridMvc.Tests.Sorting
{
    /// <summary>
    /// Mock of grid settings
    /// </summary>
    public class TestGridSortSettings : IGridSortSettings
    {
        public TestGridSortSettings(string columnName, GridSortDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }

        #region IGridSortSettings Members

        public string ColumnName { get; set; }
        public GridSortDirection Direction { get; set; }

        #endregion
    }
}