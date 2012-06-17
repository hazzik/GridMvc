namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Int64 columns
    /// </summary>
    internal class LongFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (long).FullName; }
        }

        public GridFilterType GetValidType(GridFilterType type)
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

        public object GetTypedValue(string value)
        {
            long i;
            if (!long.TryParse(value, out i))
                return null;
            return i;
        }

        #endregion
    }
}