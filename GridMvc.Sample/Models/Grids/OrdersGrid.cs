using System.Linq;
using GridMvc.Pagination;

namespace GridMvc.Sample.Models.Grids
{
    public class OrdersGrid : Grid<Order>
    {
        public OrdersGrid(IQueryable<Order> items)
            : base(items)
        {
        }
    }
}