using System;
using System.ComponentModel;
using System.Linq.Expressions;
using ToDo.Api.Contract.Shared;
using ToDo.ReadStore.Shared;

namespace ToDo.Service.Helpers;

internal static class SortingSettingsExtensions
{
    public static void AddSorting<TModel>(this SortingSettingsCollection<TModel> source, SortSpec sorting, Expression<Func<TModel, object>> selector)
    {
        var settings = new SortingSettings<TModel>
        {
          Descending  = sorting.Direction == SortSpecDirection.Desc,
          Expression = selector,
        };
        source.Add(settings);
    }
    
    public static void AddCriteria<TModel>(this FilterSettingsCollection<TModel> source, Expression<Func<TModel, object>> propertySelector, FilterSpec filterSpec)
    {
        var property = propertySelector.GetMemberExpression();
        var expression = BuildExpressionRecursive(property, filterSpec, null);
        
        if (expression is null)
        {
            throw new ArgumentException("At least one date must be assigned.", nameof(filterSpec));
        }
        
        var predicate = Expression.Lambda<Func<TModel, bool>>(expression, (ParameterExpression)property.Expression);
        
        source.Add(predicate);
    }

    private static Expression BuildExpressionRecursive(MemberExpression property, FilterSpec filterSpec, Expression expression)
    {
        var current = filterSpec;
        var result = expression;
        
        while (current is {})
        {
            result = BuildExpression(property, current, result);
            current = current.Other;
        }

        return result;
    }
    
    private static Expression BuildExpression(MemberExpression property, FilterSpec spec, Expression expression)
    {
        var value = TypeDescriptor.GetConverter(property.Type).ConvertFromInvariantString(spec.Value);
        var constantExpression = Expression.Constant(value, property.Type);
        var operatorExpression = GetExpression(spec.Operator, property, constantExpression);

        if (spec.Negate)
        {
            operatorExpression = Expression.Negate(operatorExpression);
        }

        if (expression == null)
        {
            return operatorExpression;
        }

        if (spec.LogicalOperator == FilterSpecLogicalOperator.Or)
        {
            return Expression.Or(expression, operatorExpression);
        }
        
        return Expression.And(expression, operatorExpression);
    }

    private static Expression GetExpression(FilterSpecOperator @operator, MemberExpression property, ConstantExpression value)
    {
        var empty = Expression.Constant("", typeof(string)); // todo create constant with default value of `property.Type`
        
        return @operator switch
        {
            FilterSpecOperator.Contains => Expression.Call(property, "Contains", null, value),
            FilterSpecOperator.Equals => Expression.Equal(property, value),
            FilterSpecOperator.GreaterThan => Expression.GreaterThan(property, value),
            FilterSpecOperator.GreaterOrEqualThan => Expression.GreaterThanOrEqual(property, value),
            FilterSpecOperator.LessThan => Expression.LessThan(property, value),
            FilterSpecOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, value),
            FilterSpecOperator.None => null,
            FilterSpecOperator.IsNull => Expression.Equal(property, Expression.Constant(null, typeof(object))),
            FilterSpecOperator.IsEmpty => Expression.Equal(property, empty),
            _ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null)
        };
    }
}