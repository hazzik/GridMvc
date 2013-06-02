using System.Linq;
using GridMvc.Pagination;

namespace GridMvc.Sample.Models.Grids
{
    public class OrdersAjaxPagingGrid : OrdersGrid
    {
        private readonly AjaxGridPager _pager;

        public OrdersAjaxPagingGrid(IQueryable<Order> items, int page, bool renderOnlyRows)
            : base(items)
        {
            _pager = new AjaxGridPager(this) { CurrentPage = page };
            Pager = _pager; //override  default pager
            RenderOptions.RenderRowsOnly = renderOnlyRows;
        }

        public AjaxGridPager CustomPager
        {
            get { return _pager; }
        }
    }

    public class AjaxGridPager : IGridPager
    {
        private readonly IGrid _grid;

        public AjaxGridPager(IGrid grid)
        {
            _grid = grid;
        }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public string TemplateName
        {
            get
            {
                //Custom view name to render this pager
                return "_AjaxGridPager";
            }
        }

        /// <summary>
        ///     Returns true if the pager has pages
        /// </summary>
        public bool HasPages
        {
            get
            {
                return _grid.ItemsToDisplay.Count() >= PageSize;
            }
        }

        public void Initialize<T>(IQueryable<T> items)
        {
            //do nothing
        }
    }
}