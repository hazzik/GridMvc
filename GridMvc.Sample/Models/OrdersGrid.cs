using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GridMvc.Sorting;

namespace GridMvc.Sample.Models
{
    public class OrdersGrid :Grid<Order>
    {
        public OrdersGrid(IQueryable<Order> items) : base(items)
        {

            /* Adding "OrderID" column: */
            Columns.Add(o => o.OrderID, "myid")
                    .Titled("Number")
                    .SortInitialDirection(GridSortDirection.Ascending)
                    .SetWidth(100);
            /* Adding "OrderDate" column: */
            Columns.Add(o => o.OrderDate)
                    .Titled("Date")
                    .Sortable(true)
                    .SetWidth(110);
            /* Adding "CompanyName" column: */
            Columns.Add(o => o.Customer.CompanyName)
                    .Titled("Company Name")
                    .ThenSortByDescending(o => o.OrderID)
                    .SetFilterWidgetType("CustomCompanyNameFilterWidget");

            EnablePaging = true;
            Pager.PageSize = 30;
        }
    }
}