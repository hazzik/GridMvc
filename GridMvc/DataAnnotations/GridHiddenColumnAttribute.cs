using System;
using System.Web.Mvc;
using GridMvc.Columns;

namespace GridMvc.DataAnnotations
{
    /// <summary>
    ///     Marks property as hidden Grid.Mvc column
    /// </summary>
    public class GridHiddenColumnAttribute : Attribute , IMetadataAware
    {
        public GridHiddenColumnAttribute()
        {
            EncodeEnabled = true;
            SanitizeEnabled = true;
        }

        /// <summary>
        ///     Specify that content of this column need to be encoded
        /// </summary>
        public bool EncodeEnabled { get; set; }

        /// <summary>
        ///     Specify that content of this column need to be sanitized
        /// </summary>
        public bool SanitizeEnabled { get; set; }

        /// <summary>
        ///     Specify the format of column data
        /// </summary>
        public string Format { get; set; }

        public virtual void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.DisplayFormatString = Format;
            metadata.AdditionalValues[AdditionalMetadataKeys.EncodeEnabledKey] = EncodeEnabled;
            metadata.AdditionalValues[AdditionalMetadataKeys.SanitizeEnabledKey] = SanitizeEnabled;
            metadata.AdditionalValues[AdditionalMetadataKeys.HiddenKey] = true;
        }
    }
}