using System.Linq;

namespace GridMvc.Sample.Models
{
    public interface IRepository<out T>
    {
        IOrderedQueryable<T> GetAll();
        T GetById(object id);
    }
}