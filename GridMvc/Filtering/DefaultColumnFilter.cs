using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.Filtering.Types;

namespace GridMvc.Filtering
{
    /// <summary>
    /// Default Grid.Mvc filter. Provides logic for filtering items collection.
    /// </summary>
    internal class DefaultColumnFilter<T, TData> : IColumnFilter<T>
    {
        private readonly Expression<Func<T, TData>> _expression;
        private readonly FilterTypeResolver _typeResolver = new FilterTypeResolver();

        public DefaultColumnFilter(Expression<Func<T, TData>> expression)
        {
            _expression = expression;
        }

        #region IColumnFilter<T> Members

        public IQueryable<T> ApplyFilter(IQueryable<T> items, IGridFilterSettings settings)
        {
            var pi = (PropertyInfo)((MemberExpression)_expression.Body).Member;
            Expression<Func<T, bool>> expr = GetFilterExpression(pi, settings);
            if (expr == null)
                return items;
            return items.Where(expr);
        }

        #endregion

        private Expression<Func<T, bool>> GetFilterExpression(PropertyInfo pi, IGridFilterSettings settings)
        {
            //detect nullable
            bool isNullable = pi.PropertyType.IsGenericType &&
                              pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            //get target type:
            Type targetType = isNullable ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;

            IFilterType filterType = _typeResolver.GetFilterType(targetType.FullName);
            //get typed value of query string parameter
            object typedValue = filterType.GetTypedValue(settings.Value);
            if (typedValue == null)
                return null; //incorrent filter value;
            //determine allowed filter types for property type
            GridFilterType type = filterType.GetValidType(settings.Type);

            //build expression to filter collection:
            ParameterExpression entityParam = _expression.Parameters[0];
            Expression valueExpr = Expression.Constant(typedValue);
            //support nullable types:
            Expression firstExpr = isNullable
                                       ? Expression.Property(_expression.Body, pi.PropertyType.GetProperty("Value"))
                                       : _expression.Body;

            Expression binaryExpression;
            switch (type)
            {
                case GridFilterType.Equals:
                    binaryExpression = Expression.Equal(firstExpr, valueExpr);
                    break;
                case GridFilterType.Contains:
                    MethodInfo miContains = targetType.GetMethod("Contains", new[] { typeof(string) });
                    MethodInfo miUpper = targetType.GetMethod("ToUpper", new Type[] { });
                    //case insensitive compartion:
                    var upperValueExpr = Expression.Call(valueExpr, miUpper);
                    var upperFirstExpr = Expression.Call(firstExpr, miUpper);
                    binaryExpression = Expression.Call(upperFirstExpr, miContains, upperValueExpr);
                    break;
                case GridFilterType.StartsWith:
                    MethodInfo miStartsWith = targetType.GetMethod("StartsWith", new[] { typeof(string) });
                    binaryExpression = Expression.Call(firstExpr, miStartsWith, valueExpr);
                    break;
                case GridFilterType.EndsWidth:
                    MethodInfo miEndssWith = targetType.GetMethod("EndsWith", new[] { typeof(string) });
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

            if (targetType == typeof(string))
            {
                //check for strings, they may be NULL
                //It's ok for ORM, but throw exception in linq to objects. Additional check string on null
                Expression nullExpr = Expression.NotEqual(_expression.Body, Expression.Constant(null));
                binaryExpression = Expression.AndAlso(nullExpr, binaryExpression);
            }
            else if (isNullable)
            {
                //add additional filter condition for check items on NULL with invoring "HasValue" method.
                //for example: result of this expression will like - c=> c.HasValue && c.Value = 3
                MemberExpression hasValueExpr = Expression.Property(_expression.Body,
                                                                    pi.PropertyType.GetProperty("HasValue"));
                binaryExpression = Expression.AndAlso(hasValueExpr, binaryExpression);
            }
            //return filter expression
            return Expression.Lambda<Func<T, bool>>(binaryExpression, entityParam);
        }
    }
}