namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Int32 columns
    /// </summary>
    internal class IntegerFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (int).FullName; }
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
            int i;
            if (!int.TryParse(value, out i))
                return null;
            return i;
        }

        #endregion
    }
}