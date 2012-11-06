using System;
using System.Globalization;

namespace GridMvc.Filtering.Types
{
    /// <summary>
    /// Object contains some logic for filtering DateTime columns
    /// </summary>
    internal sealed class DateTimeFilterType : FilterTypeBase
    {
        public override Type TargetType
        {
            get { return typeof (DateTime); }
        }

        /// <summary>
        /// There are filter types that allowed for DateTime column
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GridFilterType GetValidType(GridFilterType type)
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

        public override object GetTypedValue(string value)
        {
            DateTime date;
            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return null;
            return date;
        }
    }
}