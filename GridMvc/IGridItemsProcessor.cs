using System.Linq;

namespace GridMvc
{
    /// <summary>
    /// Propocess items to display
    /// </summary>
    public interface IGridItemsProcessor<T> where T : class
    {
        IQueryable<T> Process(IQueryable<T> items);
    }
}