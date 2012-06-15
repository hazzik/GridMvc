namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for date grid columns
    /// </summary>
    internal class DateTimeFilterTypeSanitizer : IFilterTypeSanitizer
    {
        #region IFilterTypeSanitizer Members

        public GridFilterType SanitizeType(GridFilterType type)
        {
            switch (type)
            {
                case GridFilterType.Equals:
                case GridFilterType.GreaterThan:
                case GridFilterType.LessThan:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        #endregion
    }
}