using System;
using GridMvc.Filtering;
using GridMvc.Sorting;

namespace GridMvc.Tests.Sorting
{
    /// <summary>
    /// Mock of sort provider for the grid
    /// </summary>
    internal class TestSettingsProvider : IGridSettingsProvider
    {
        public TestSettingsProvider()
        {
        }

        public TestSettingsProvider(string columnName, GridSortDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }

        public string ColumnName { get; set; }
        public GridSortDirection Direction { get; set; }

        #region IGridSettingsProvider Members

        public GridHeaderRenderer HeaderRenderer
        {
            get { throw new NotImplementedException(); }
        }

        public IGridSortSettings SortSettings
        {
            get { return new TestGridSortSettings(ColumnName, Direction); }
        }

        public IGridFilterSettings FilterSettings
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}