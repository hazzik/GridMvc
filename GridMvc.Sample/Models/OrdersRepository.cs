using System.Linq;

namespace GridMvc.Sample.Models
{
    public class OrdersRepository : SqlRepository<Order>
    {
        public OrdersRepository()
            : base(new NorthwindDbContext())
        {
        }

        public override IOrderedQueryable<Order> GetAll()
        {
            return EfDbSet.Include("Customer").OrderByDescending(o => o.OrderDate);
        }

        public override Order GetById(object id)
        {
            return GetAll().FirstOrDefault(o => o.OrderID == (int) id);
        }
    }
}