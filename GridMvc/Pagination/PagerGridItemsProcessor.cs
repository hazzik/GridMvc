using System.Linq;

namespace GridMvc.Pagination
{
    /// <summary>
    /// Cut's the current page from items collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private readonly IGridPager _pager;

        public PagerGridItemsProcessor(IGrid grid, IGridPager pager)
        {
            _grid = grid;
            _pager = pager;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            _pager.ItemsCount = items.Count(); //take current items count
            _grid.ItemsCount = _pager.ItemsCount;
            if (_pager.CurrentPage <= 0) return items; //incorrect page

            int skip = (_pager.CurrentPage - 1)*_pager.PageSize;
            return items.Skip(skip).Take(_pager.PageSize);
        }

        #endregion
    }
}