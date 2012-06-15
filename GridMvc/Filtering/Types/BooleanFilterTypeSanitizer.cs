namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for boolean grid columns
    /// </summary>
    internal class BooleanFilterTypeSanitizer : IFilterTypeSanitizer
    {
        #region IFilterTypeSanitizer Members

        public GridFilterType SanitizeType(GridFilterType type)
        {
            return GridFilterType.Equals;
        }

        #endregion
    }
}