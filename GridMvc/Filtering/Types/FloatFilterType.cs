namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Float columns
    /// </summary>
    internal class FloatFilterType : IFilterType
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
            float flt;
            if (!float.TryParse(value, out flt))
                return null;
            return flt;
        }

        #endregion
    }
}