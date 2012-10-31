using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GridMvc.Utility
{
    /// <summary>
    /// Helper class for reflection operations
    /// </summary>
    internal static class PropertiesHelper
    {
        private const string PropertiesQueryStringDelimeter = "__";

        public static string BuildColumnNameFromMemberExpression(MemberExpression memberExpr)
        {
            var sb = new StringBuilder();
            Expression expr = memberExpr;
            while (expr is MemberExpression)
            {
                var typed = expr as MemberExpression;
                if (sb.Length > 0)
                    sb.Insert(0, PropertiesQueryStringDelimeter);
                sb.Insert(0, typed.Member.Name);
                expr = typed.Expression;
            }
            return sb.ToString();
        }



        public static PropertyInfo GetPropertyFromColumnName(string columnName, Type type,
                                                             out IEnumerable<PropertyInfo> propertyInfoSequence)
        {
            string[] properies = columnName.Split(new[] {PropertiesQueryStringDelimeter},
                                                  StringSplitOptions.RemoveEmptyEntries);
            if (!properies.Any())
            {
                propertyInfoSequence = null;
                return null;
            }
            PropertyInfo pi = null;
            var sequence = new List<PropertyInfo>();
            foreach (string properyName in properies)
            {
                pi = type.GetProperty(properyName);
                if (pi == null)
                {
                    propertyInfoSequence = null;
                    return null; //no match column
                }
                sequence.Add(pi);
                type = pi.PropertyType;
            }
            propertyInfoSequence = sequence;
            return pi;
        }

        public static Type GetUnderlyingType(Type type)
        {
            Type targetType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }
            else
            {
                targetType = type;
            }
            return targetType;
        }

        public static T GetAttribute<T>(this PropertyInfo pi)
        {
            return (T)pi.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }

        public static T GetAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }
    }
}