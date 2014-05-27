using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GridMvc.Utility;

namespace GridMvc.DataAnnotations
{
    internal class GridAnnotaionsProvider : IGridAnnotaionsProvider
    {
        public bool IsColumnMapped(PropertyInfo pi)
        {
            return pi.GetAttribute<NotMappedColumnAttribute>() == null;
        }

        public GridTableAttribute GetAnnotationForTable<T>()
        {
            var modelType = typeof(T).GetAttribute<MetadataTypeAttribute>();
            if (modelType != null)
            {
                var metadataAttr = modelType.MetadataClassType.GetAttribute<GridTableAttribute>();
                if (metadataAttr != null)
                    return metadataAttr;
            }
            return typeof(T).GetAttribute<GridTableAttribute>();
        }
    }
}