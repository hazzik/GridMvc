using System;

namespace GridMvc.DataAnnotations
{
    /// <summary>
    /// Marks property as hidden Grid.Mvc column
    /// </summary>
    public class GridHiddenColumnAttribute : Attribute
    {
        public GridHiddenColumnAttribute()
        {
            Encoded = true;
            Sanitized = true;
        }

        /// <summary>
        /// Specify that content of this column need to be encoded
        /// </summary>
        public bool Encoded { get; set; }

        /// <summary>
        /// Specify that content of this column need to be sanitized
        /// </summary>
        public bool Sanitized { get; set; }

        /// <summary>
        /// Specify the format of column data
        /// </summary>
        public string Format { get; set; }
    }
}
