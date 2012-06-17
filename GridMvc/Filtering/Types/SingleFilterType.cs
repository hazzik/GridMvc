using System;

namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering Single columns
    /// </summary>
    internal class SingleFilterType : IFilterType
    {
        #region IFilterType Members

        public string TypeName
        {
            get { return typeof (Single).FullName; }
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
            Single sng;
            if (!Single.TryParse(value, out sng))
                return null;
            return sng;
        }

        #endregion
    }
}