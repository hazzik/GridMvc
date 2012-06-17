namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object builds filter expressions for boolean grid columns
    /// </summary>
    internal class BooleanFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (bool).FullName; }
        }

        public GridFilterType GetValidType(GridFilterType type)
        {
            //in any case Boolean types must compare by Equals filter type
            //We can't compare: contains(true) and etc.
            return GridFilterType.Equals;
        }

        public object GetTypedValue(string value)
        {
            bool b;
            if (!bool.TryParse(value, out b))
                return null;
            return b;
        }

        #endregion
    }
}