using System;
using GridMvc.Sorting;

namespace GridMvc.Tests.Sorting
{
    /// <summary>
    /// Mock of sort provider for the grid
    /// </summary>
    internal class TestSortProvider : IGridSortProvider
    {
        public TestSortProvider()
        {
        }

        public TestSortProvider(string columnName, GridSortDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }

        public string ColumnName { get; set; }
        public GridSortDirection Direction { get; set; }

        #region IGridSortProvider Members

        public IGridColumnRenderer HeaderRenderer
        {
            get { throw new NotImplementedException(); }
        }

        public IGridSortSettings Settings
        {
            get { return new TestGridSortSettings(ColumnName, Direction); }
        }

        #endregion
    }
}