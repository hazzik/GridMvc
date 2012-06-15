using System.Data.Objects;
using System.Linq;

namespace GridMvc.Sample.Models
{
    public abstract class SqlRepository<T> : IRepository<T> where T : class
    {
        protected readonly ObjectSet<T> EFDbSet;

        protected SqlRepository(ObjectContext context)
        {
            EFDbSet = context.CreateObjectSet<T>();
        }

        #region IRepository<T> Members

        public virtual IOrderedQueryable<T> GetAll()
        {
            return EFDbSet;
        }

        public abstract T GetById(object id);

        #endregion
    }
}