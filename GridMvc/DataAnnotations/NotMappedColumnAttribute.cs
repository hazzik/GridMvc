using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridMvc.DataAnnotations
{
    /// <summary>
    /// Marks property as not a column. Grid.Mvc will not add this property to the column
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMappedColumnAttribute : Attribute
    {
    }
}
