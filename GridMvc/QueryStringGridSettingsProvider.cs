using GridMvc.Filtering;
using GridMvc.Sorting;

namespace GridMvc
{
    /// <summary>
    /// Provider of grid settings, based on query string parameters
    /// </summary>
    public class QueryStringGridSettingsProvider : IGridSettingsProvider
    {
        private readonly QueryStringFilterSettings _filterSettings;
        private readonly QueryStringSortColumnHeaderRenderer _headerRenderer;
        private readonly QueryStringSortSettings _sortSettings;

        public QueryStringGridSettingsProvider()
        {
            _sortSettings = new QueryStringSortSettings();
            _headerRenderer = new QueryStringSortColumnHeaderRenderer(_sortSettings);
            //add additional header renderer for filterable columns:
            _filterSettings = new QueryStringFilterSettings();
            _headerRenderer.AddAdditionalRenderer(new QueryStringFilterColumnHeaderRenderer(_filterSettings));
        }

        #region IGridSettingsProvider Members

        public GridHeaderRenderer HeaderRenderer
        {
            get { return _headerRenderer; }
        }

        public IGridSortSettings SortSettings
        {
            get { return _sortSettings; }
        }

        #endregion
    }
}