using System.Linq;
using GridMvc.Columns;
using GridMvc.Pagination;
using GridMvc.Sorting;

namespace GridMvc
{
    /// <summary>
    /// Base grid class
    /// </summary>
    public class Grid<T> : GridBase<T>, IGrid where T : class
    {
        private readonly GridColumnCollection<T> _columnsCollection;
        private SortGridItemsProcessor<T> _currentSortItemsProcessor;

        private int _displayingItemsCount = -1; // count of displaying items (if using pagination)
        private bool _enablePaging;
        private IGridPager _pager;

        private IGridItemsProcessor<T> _pagerProcessor;
        private IGridSortProvider _sortingProvider;

        public Grid(IQueryable<T> items)
            : base(items)
        {
            #region init default properties

            //set up sort settings:
            Sorting = new QueryStringGridSortProvider();
            Sanitizer = new Sanitizer();

            #endregion

            //Set up column collection:
            var columnBuilder = new DefaultColumnBuilder(this);
            _columnsCollection = new GridColumnCollection<T>(columnBuilder, Sorting);
        }

        /// <summary>
        /// Grid columns collection
        /// </summary>
        public IGridColumnCollection<T> Columns
        {
            get { return _columnsCollection; }
        }

        /// <summary>
        /// Sets or get default sorting for all adding columns
        /// </summary>
        public bool DefaultSortEnabled
        {
            get { return _columnsCollection.DefaultSortEnabled; }
            set { _columnsCollection.DefaultSortEnabled = value; }
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
                        _pagerProcessor = new PagerGridItemsProcessor<T>(Pager);
                    AddItemsProcessor(_pagerProcessor);
                }
                else
                {
                    RemoveItemsProcessor(_pagerProcessor);
                }
            }
        }

        /// <summary>
        /// Provides the sort provider, using by the grid
        /// </summary>
        public IGridSortProvider Sorting
        {
            get { return _sortingProvider; }
            set
            {
                _sortingProvider = value;
                RemoveItemsProcessor(_currentSortItemsProcessor);
                _currentSortItemsProcessor = new SortGridItemsProcessor<T>(this, _sortingProvider.Settings);
                InsertItemsProcessor(0, _currentSortItemsProcessor);
            }
        }

        /// <summary>
        /// Gets or set Grid column values sanitizer
        /// </summary>
        public ISanitizer Sanitizer { get; set; }

        /// <summary>
        /// Manager pager properties
        /// </summary>
        public IGridPager Pager
        {
            get { return _pager ?? (_pager = new GridPager {ItemsCount = ItemsCount}); }
            set { _pager = value; }
        }

        IGridColumnCollection IGrid.Columns
        {
            get { return _columnsCollection; }
        }

        #endregion
    }
}