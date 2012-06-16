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
            //in any case Boolean types must compare by Equals filter type
            //We can't compare: contains(true) and etc.
            return GridFilterType.Equals;
        }

        #endregion
    }
}