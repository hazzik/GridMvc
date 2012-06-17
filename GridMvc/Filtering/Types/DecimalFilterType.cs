namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering decimal columns
    /// </summary>
    internal class DecimalFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (float).FullName; }
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
            decimal dec;
            if (!decimal.TryParse(value, out dec))
                return null;
            return dec;
        }

        #endregion
    }
}