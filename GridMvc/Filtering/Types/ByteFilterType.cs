namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Byte columns
    /// </summary>
    internal class ByteFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (byte).FullName; }
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
            byte bt;
            if (!byte.TryParse(value, out bt))
                return null;
            return bt;
        }

        #endregion
    }
}