using System.Reflection;

namespace GridMvc.DataAnnotations
{
    internal interface IGridAnnotaionsProvider
    {
        bool IsColumnMapped(PropertyInfo pi);

        GridTableAttribute GetAnnotationForTable<T>();
    }
}