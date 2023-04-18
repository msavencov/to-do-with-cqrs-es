using System;
using Radzen;
using ToDo.Api.Contract.Shared;

namespace ToDo.App.Helpers.UI;

public static class GridViewExtensions
{
    public static FilterSpecOperator GetSpecOperator(this FilterDescriptor descriptor, out bool negate)
    {
        negate = false;
        
        switch (descriptor.FilterOperator)
        {
            case FilterOperator.Equals:
            case FilterOperator.NotEquals:
                negate = true;
                return FilterSpecOperator.Equals;
            case FilterOperator.LessThan:
                return FilterSpecOperator.LessThan;
            case FilterOperator.LessThanOrEquals:
                return FilterSpecOperator.LessThanOrEqual;
            case FilterOperator.GreaterThan:
                return FilterSpecOperator.GreaterThan;
            case FilterOperator.GreaterThanOrEquals:
                return FilterSpecOperator.GreaterOrEqualThan;
            case FilterOperator.Contains:
            case FilterOperator.StartsWith:
            case FilterOperator.EndsWith:
                return FilterSpecOperator.Contains;
            case FilterOperator.DoesNotContain:
                negate = true;
                return FilterSpecOperator.Contains;
            case FilterOperator.IsNull:
                return FilterSpecOperator.IsNull;
            case FilterOperator.IsEmpty:
                return FilterSpecOperator.IsEmpty;
            case FilterOperator.IsNotNull:
                negate = true;
                return FilterSpecOperator.IsNull;
            case FilterOperator.IsNotEmpty:
                negate = true;
                return FilterSpecOperator.IsEmpty;
            default:
                throw new ArgumentOutOfRangeException(nameof(descriptor), descriptor.FilterOperator, null);
        }
    }
}