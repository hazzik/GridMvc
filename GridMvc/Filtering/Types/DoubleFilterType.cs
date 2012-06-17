namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Double columns
    /// </summary>
    internal class DoubleFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (double).FullName; }
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
            double db;
            if (!double.TryParse(value, out db))
                return null;
            return db;
        }

        #endregion
    }
}