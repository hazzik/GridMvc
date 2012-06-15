using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.Filtering.Types;

namespace GridMvc.Filtering
{
    internal class DefaultColumnFilter<T, TData> : IColumnFilter<T>
    {
        private readonly Expression<Func<T, TData>> _expression;

        public DefaultColumnFilter(Expression<Func<T, TData>> expression)
        {
            _expression = expression;
        }

        #region IColumnFilter<T> Members

        public IQueryable<T> ApplyFilter(IQueryable<T> items, string value, GridFilterType type)
        {
            var pi = (PropertyInfo) ((MemberExpression) _expression.Body).Member;
            Expression<Func<T, bool>> expr = GetFilterExpression(pi, value, type);
            if (expr == null)
                return items;
            return items.Where(expr);
        }

        #endregion

        private Expression<Func<T, bool>> GetFilterExpression(PropertyInfo pi, string value, GridFilterType type)
        {
            //detect nullable
            bool isNullable = pi.PropertyType.IsGenericType &&
                              pi.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
            //get target type TODO: refactor
            Type targetType = isNullable ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;
            //get typed value of query string parameter
            object typedValue = ConvertToType(value, targetType);
            if (typedValue == null)
                return null; //incorrent filter value;

            //determine allowed filter types for property type
            IFilterTypeSanitizer sanitizer = FilterTypeResolver.GetSanitizer(targetType.FullName);
            type = sanitizer.SanitizeType(type);

            ParameterExpression entityParam = _expression.Parameters[0];

            Expression valueExpr = Expression.Constant(typedValue);
            Expression firstExpr;
            Expression binaryExpression;
            //support nullable types:
            if (isNullable)
            {
                firstExpr = Expression.Property(_expression.Body, pi.PropertyType.GetProperty("Value"));
            }
            else
            {
                firstExpr = _expression.Body;
            }

            switch (type)
            {
                case GridFilterType.Equals:
                    binaryExpression = Expression.Equal(firstExpr, valueExpr);
                    break;
                case GridFilterType.Contains:
                    MethodInfo miContains = targetType.GetMethod("Contains", new[] {typeof (string)});
                    binaryExpression = Expression.Call(firstExpr, miContains, valueExpr);
                    break;
                case GridFilterType.StartsWith:
                    MethodInfo miStartsWith = targetType.GetMethod("StartsWith", new[] {typeof (string)});
                    binaryExpression = Expression.Call(firstExpr, miStartsWith, valueExpr);
                    break;
                case GridFilterType.EndsWidth:
                    MethodInfo miEndssWith = targetType.GetMethod("EndsWith", new[] {typeof (string)});
                    binaryExpression = Expression.Call(firstExpr, miEndssWith, valueExpr);
                    break;
                case GridFilterType.LessThan:
                    binaryExpression = Expression.LessThan(firstExpr, valueExpr);
                    break;
                case GridFilterType.GreaterThan:
                    binaryExpression = Expression.GreaterThan(firstExpr, valueExpr);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isNullable)
            {
                MemberExpression hasValueExpr = Expression.Property(_expression.Body,
                                                                    pi.PropertyType.GetProperty("HasValue"));
                binaryExpression = Expression.AndAlso(hasValueExpr, binaryExpression);
            }

            return Expression.Lambda<Func<T, bool>>(binaryExpression, entityParam);
        }

        private object ConvertToType(string value, Type targetType)
        {
            switch (targetType.FullName)
            {
                case "System.Int32":
                    int i;
                    if (!int.TryParse(value, out i))
                        return null;
                    return i;
                case "System.String":
                    return value;
                case "System.Boolean":
                    bool b;
                    if (!bool.TryParse(value, out b))
                        return null;
                    return b;
                case "System.Byte":
                    byte bt;
                    if (!byte.TryParse(value, out bt))
                        return null;
                    return bt;
                case "System.Single":
                    Single sng;
                    if (!Single.TryParse(value, out sng))
                        return null;
                    return sng;
                case "System.Double":
                    double db;
                    if (!double.TryParse(value, out db))
                        return null;
                    return db;
                case "System.Decimal":
                    decimal dec;
                    if (!decimal.TryParse(value, out dec))
                        return null;
                    return dec;
                case "System.Float":
                    float flt;
                    if (!float.TryParse(value, out flt))
                        return null;
                    return flt;
                case "System.DateTime":
                    DateTime t;
                    if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                        return null;
                    return t;
                default:
                    throw new InvalidOperationException(
                        string.Format("Column of type '{0}' does not support for filtering", targetType.FullName));
            }
        }
    }
}