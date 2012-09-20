using System.Linq;
using GridMvc.Columns;
using GridMvc.Filtering;
using GridMvc.Pagination;
using GridMvc.Resources;
using GridMvc.Sorting;

namespace GridMvc
{
    /// <summary>
    /// Grid.Mvc base class
    /// </summary>
    public class Grid<T> : GridBase<T>, IGrid where T : class
    {
        private readonly GridColumnCollection<T> _columnsCollection;
        private SortGridItemsProcessor<T> _currentSortItemsProcessor;

        private int _displayingItemsCount = -1; // count of displaying items (if using pagination)
        private bool _enablePaging;
        private IGridPager _pager;

        private IGridItemsProcessor<T> _pagerProcessor;
        private IGridSettingsProvider _settings;

        public Grid(IQueryable<T> items)
            : base(items)
        {
            AddItemsPreProcessor(new FilterGridItemsProcessor<T>(this, new QueryStringFilterSettings()));

            #region init default properties

            //set up sort settings:
            Settings = new QueryStringGridSettingsProvider();
            Sanitizer = new Sanitizer();
            EmptyGridText = Strings.DefaultGridEmptyText;
            Language = Strings.Lang;

            #endregion

            //Set up column collection:
            var columnBuilder = new DefaultColumnBuilder<T>(this);
            _columnsCollection = new GridColumnCollection<T>(columnBuilder);
        }

        /// <summary>
        /// Grid columns collection
        /// </summary>
        public IGridColumnCollection<T> Columns
        {
            get { return _columnsCollection; }
        }

        /// <summary>
        /// Sets or get default value of sorting for all adding columns
        /// </summary>
        public bool DefaultSortEnabled
        {
            get { return _columnsCollection.DefaultSortEnabled; }
            set { _columnsCollection.DefaultSortEnabled = value; }
        }

        /// <summary>
        /// Set or get default value of filtering for all adding columns
        /// </summary>
        public bool DefaultFilteringEnabled
        {
            get { return _columnsCollection.DefaultFilteringEnabled; }
            set { _columnsCollection.DefaultFilteringEnabled = value; }
        }

        /// <summary>
        /// Provides settings, using by the grid
        /// </summary>
        public IGridSettingsProvider Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                RemoveItemsProcessor(_currentSortItemsProcessor);
                _currentSortItemsProcessor = new SortGridItemsProcessor<T>(this, _settings.SortSettings);
                InsertItemsProcessor(0, _currentSortItemsProcessor);
            }
        }

        #region IGrid Members

        /// <summary>
        /// Count of current displaying items
        /// </summary>
        public int DisplayingItemsCount
        {
            get
            {
                if (_displayingItemsCount >= 0)
                    return _displayingItemsCount;
                _displayingItemsCount = ItemsToDisplay.Count();
                return _displayingItemsCount;
            }
        }

        /// <summary>
        /// Enable or disable paging for the grid
        /// </summary>
        public bool EnablePaging
        {
            get { return _enablePaging; }
            set
            {
                if (_enablePaging == value) return;
                _enablePaging = value;
                if (_enablePaging)
                {
                    if (_pagerProcessor == null)
                        _pagerProcessor = new PagerGridItemsProcessor<T>(this, Pager);
                    AddItemsProcessor(_pagerProcessor);
                }
                else
                {
                    RemoveItemsProcessor(_pagerProcessor);
                }
            }
        }

        public string Language { get; set; }

        /// <summary>
        /// Gets or set Grid column values sanitizer
        /// </summary>
        public ISanitizer Sanitizer { get; set; }

        public void OnPreRender()
        {
            ProcessColumns();
            ProcessItemsToDisplay();
        }

        /// <summary>
        /// Manage pager properties
        /// </summary>
        public IGridPager Pager
        {
            get { return _pager ?? (_pager = new GridPager()); }
            set { _pager = value; }
        }

        IGridColumnCollection IGrid.Columns
        {
            get { return _columnsCollection; }
        }

        private void ProcessColumns()
        {
            if (!string.IsNullOrEmpty(Settings.SortSettings.ColumnName))
            {
                foreach (IGridColumn gridColumn in Columns)
                {
                    gridColumn.IsSorted = gridColumn.Name == Settings.SortSettings.ColumnName;
                    if (gridColumn.Name == Settings.SortSettings.ColumnName)
                        gridColumn.Direction = Settings.SortSettings.Direction;
                    else
                        gridColumn.Direction = null;
                }
            }
        }

        #endregion
    }
}