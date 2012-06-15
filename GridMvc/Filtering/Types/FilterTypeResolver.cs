using System;

namespace GridMvc.Filtering.Types
{
    internal class FilterTypeResolver
    {
        public static IFilterTypeSanitizer GetSanitizer(string typeName)
        {
            switch (typeName)
            {
                case "System.String":
                    return new TextFilterTypeSanitizer();
                case "System.Int32":
                case "System.Double":
                case "System.Byte":
                case "System.Decimal":
                case "System.Single":
                    return new NumberFilterTypeSanitizeer();
                case "System.DateTime":
                    return new DateTimeFilterTypeSanitizer();
                case "System.Boolean":
                    return new DateTimeFilterTypeSanitizer();
                default:
                    throw new ArgumentException(string.Format("Filtering of type '{0}' not supported", typeName));
            }
        }
    }
}