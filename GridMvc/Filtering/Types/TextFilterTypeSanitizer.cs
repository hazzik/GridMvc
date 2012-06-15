namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for text (string) grid columns
    /// </summary>
    internal class TextFilterTypeSanitizer : IFilterTypeSanitizer
    {
        #region IFilterTypeSanitizer Members

        public GridFilterType SanitizeType(GridFilterType type)
        {
            switch (type)
            {
                case GridFilterType.Equals:
                case GridFilterType.Contains:
                case GridFilterType.StartsWith:
                case GridFilterType.EndsWidth:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        #endregion
    }
}