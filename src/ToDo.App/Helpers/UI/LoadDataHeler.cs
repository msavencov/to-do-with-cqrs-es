using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Radzen;
using ToDo.Api.Contract.Shared;
using Type = System.Type;

namespace ToDo.App.Helpers.UI;

public class LoadDataHelper<TRow>
{
    private readonly LoadDataArgs _args;

    public LoadDataHelper(LoadDataArgs args)
    {
        _args = args;
    }

    public bool TryGetFilter(Expression<Func<TRow, object>> propertySelector, out FilterSpec filter)
    {
        filter = null;
        
        var property = GetPropertyInfo(propertySelector);
        var filterDescriptor = _args.Filters.FirstOrDefault(t => t.Property == property.Member.Name);
        
        if (filterDescriptor == null)
        {
            return false;
        }
        
        filter = new FilterSpec
        {
            Operator = filterDescriptor.GetSpecOperator(out var negate),
            Negate = negate,
        };

        if (filterDescriptor.FilterValue is { })
        {
            filter.Value = ConvertToString(property.Type, filterDescriptor.FilterValue);
        }

        if (filterDescriptor.SecondFilterValue is {})
        {
            filter.Other = new FilterSpec
            {
                Operator = filterDescriptor.GetSpecOperator(out var secondNegate),
                Negate = secondNegate,
                Value = ConvertToString(property.Type, filterDescriptor.SecondFilterValue),
            };
            if (filterDescriptor.LogicalFilterOperator == LogicalFilterOperator.Or)
            {
                filter.Other.LogicalOperator = FilterSpecLogicalOperator.Or;
            }
        }
        
        return true;
    }
    
    private string ConvertToString(Type type, object value)
    {
        return TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);
    }
    
    private MemberExpression GetPropertyInfo(Expression<Func<TRow,object>> expression)
    {
        var propertyExpression = expression.Body;
        
        if (expression.Body is UnaryExpression unaryExpression)
        {
            propertyExpression = unaryExpression.Operand;
        }

        return propertyExpression as MemberExpression;
    }
}