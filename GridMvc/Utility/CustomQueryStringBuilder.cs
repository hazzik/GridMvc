using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace GridMvc.Utility
{
    /// <summary>
    /// Class retain the current query string parameters
    /// </summary>
    internal class CustomQueryStringBuilder : NameValueCollection
    {
        public CustomQueryStringBuilder(NameValueCollection collection, string parameterName)
            : base(collection)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();
            if (base.AllKeys.Count() != 0) result.Append("?");
            foreach (string key in base.AllKeys)
            {
                string[] values = base.GetValues(key);
                if (values != null && values.Count() != 0)
                {
                    result.Append(key + "=" + values[0] + "&");
                }
            }
            string resultString = result.ToString();
            return resultString.EndsWith("&") ? resultString.Substring(0, resultString.Length - 1) : resultString;
        }

        public string GetQueryStringForParameter(string parameterValue)
        {
            if (string.IsNullOrEmpty(ParameterName))
                throw new Exception("ParameterName not specified");


            if (base[ParameterName] != null)
                base[ParameterName] = parameterValue;
            else
                base.Add(ParameterName, parameterValue);

            return ToString();
        }
    }
}