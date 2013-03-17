using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GridMvc.Sorting;

namespace GridMvc.Sample.Models
{
    public class OrdersGrid : Grid<Order>
    {
        public OrdersGrid(IQueryable<Order> items)
            : base(items)
        {
            Id = "myGrid";
        }

    }
}