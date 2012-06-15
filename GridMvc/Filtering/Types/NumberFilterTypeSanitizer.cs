namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for number grid columns
    /// </summary>
    internal class NumberFilterTypeSanitizeer : IFilterTypeSanitizer
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