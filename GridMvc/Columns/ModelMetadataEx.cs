using System.Web.Mvc;

namespace GridMvc.Columns
{
    internal static class ModelMetadataEx
    {
        public static T GetAdditionalValue<T>(this ModelMetadata metadata, string key)
        {
            object result;
            if (metadata.AdditionalValues.TryGetValue(key, out result) && result is T)
                return (T) result;
            return default(T);
        }
    }
}