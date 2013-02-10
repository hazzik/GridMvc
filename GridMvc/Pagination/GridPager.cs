using System;
using System.Globalization;
using System.Web;
using GridMvc.Utility;

namespace GridMvc.Pagination
{
    /// <summary>
    ///     Default grid pager implementation
    /// </summary>
    public class GridPager : IGridPager
    {
        public const int DefaultMaxDisplayedPages = 5;
        public const int DefaultPageSize = 20;
        public const string DefaultPageQueryParameter = "grid-page";
        private readonly HttpContext _context;
        private readonly CustomQueryStringBuilder _queryBuilder;
        private int _itemsCount;

        private int _maxDisplayedPages;
        private int _pageSize;

        public GridPager()
            : this(HttpContext.Current)
        {
        }

        public GridPager(HttpContext context)
        {
            _context = context;
            if (_context == null)
                throw new Exception("No http context here!");

            _queryBuilder = new CustomQueryStringBuilder(HttpContext.Current.Request.QueryString);

            MaxDisplayedPages = MaxDisplayedPages;
            ParameterName = DefaultPageQueryParameter;
            PageSize = DefaultPageSize;
        }

        #region IGridPager Members

        public string ParameterName { get; set; }

        public int ItemsCount
        {
            get { return _itemsCount; }
            set
            {
                _itemsCount = value;
                RecalculatePages();
            }
        }

        public int MaxDisplayedPages
        {
            get { return _maxDisplayedPages == 0 ? DefaultMaxDisplayedPages : _maxDisplayedPages; }
            set
            {
                _maxDisplayedPages = value;
                RecalculatePages();
            }
        }

        /// <summary>
        ///     Количество элементов на странице
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RecalculatePages();
            }
        }


        public int PageCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int StartDisplayedPage { get; private set; }
        public int EndDisplayedPage { get; private set; }

        public string GetLinkForPage(int pageIndex)
        {
            return _queryBuilder.GetQueryStringWithParameter(ParameterName,
                                                             pageIndex.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        private void RecalculatePages()
        {
            PageCount = (int) (Math.Ceiling(ItemsCount/(double) PageSize));

            string currentPageString = _context.Request.QueryString[ParameterName] ?? "1";
            int page;
            if (!int.TryParse(currentPageString, out page))
                page = 1;
            CurrentPage = page;

            if (CurrentPage > PageCount)
                CurrentPage = PageCount;

            StartDisplayedPage = (CurrentPage - MaxDisplayedPages/2) < 1
                                     ? 1
                                     : CurrentPage - MaxDisplayedPages/2;
            EndDisplayedPage = (CurrentPage + MaxDisplayedPages/2) > PageCount
                                   ? PageCount
                                   : CurrentPage + MaxDisplayedPages/2;
        }
    }
}