using System.Linq;

namespace GridMvc.Sample.Models
{
    public class OrdersGrid : Grid<Order>
    {
        public OrdersGrid(IQueryable<Order> items)
            : base(items)
        {
            RenderOptions.GridId = "myGrid";
        }

    }
}