namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for text (string) grid columns
    /// </summary>
    internal class TextFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (string).FullName; }
        }

        public GridFilterType GetValidType(GridFilterType type)
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

        public object GetTypedValue(string value)
        {
            return value;
        }

        #endregion
    }
}