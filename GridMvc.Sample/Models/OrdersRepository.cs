using System.Linq;

namespace GridMvc.Sample.Models
{
    public class OrdersRepository : SqlRepository<Orders>
    {
        public OrdersRepository()
            : base(new NorthwindEntities())
        {
        }

        public override IOrderedQueryable<Orders> GetAll()
        {
            return EFDbSet.Include("Customers").OrderByDescending(o => o.OrderDate);
        }

        public override Orders GetById(object id)
        {
            return GetAll().FirstOrDefault(o => o.OrderID == (int) id);
        }
    }
}