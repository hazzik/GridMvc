namespace GridMvc.Filtering.Types
{
    internal interface IFilterTypeSanitizer
    {
        /// <summary>
        /// Sanitize filter type for specific column data type
        /// </summary>
        /// <param name="type">Filter type (equals, contains etc)</param>
        /// <returns>Sanitized filter type</returns>
        GridFilterType SanitizeType(GridFilterType type);
    }
}