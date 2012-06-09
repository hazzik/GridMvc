namespace GridMvc.Sorting
{
    /// <summary>
    /// Provider for  sorting, based on query string parameters
    /// </summary>
    public class QueryStringGridSortProvider : IGridSortProvider
    {
        private readonly QueryStringSortColumnHeaderRenderer _headerRenderer;
        private readonly QueryStringSortSettings _settings;

        public QueryStringGridSortProvider()
        {
            _settings = new QueryStringSortSettings();
            _headerRenderer = new QueryStringSortColumnHeaderRenderer(_settings);
        }

        #region IGridSortProvider Members

        public IGridColumnRenderer HeaderRenderer
        {
            get { return _headerRenderer; }
        }

        public IGridSortSettings Settings
        {
            get { return _settings; }
        }

        #endregion
    }
}