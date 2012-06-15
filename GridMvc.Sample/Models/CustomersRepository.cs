using System.Linq;

namespace GridMvc.Sample.Models
{
    public class CustomersRepository : SqlRepository<Customers>
    {
        public CustomersRepository()
            : base(new NorthwindEntities())
        {
        }

        public override IOrderedQueryable<Customers> GetAll()
        {
            return base.GetAll().OrderBy(o => o.CompanyName);
        }

        public override Customers GetById(object id)
        {
            return GetAll().FirstOrDefault(c => c.CustomerID == (string)id);
        }
    }
}