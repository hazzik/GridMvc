using System.Linq;

namespace GridMvc.Filtering
{
    public interface IColumnFilter<T>
    {
        IQueryable<T> ApplyFilter(IQueryable<T> items, IGridFilterSettings filterSettings);
    }
}