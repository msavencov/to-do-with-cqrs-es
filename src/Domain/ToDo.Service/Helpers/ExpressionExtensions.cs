using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ToDo.Service.Helpers;

public static class ExpressionExtensions
{
    public static MemberExpression GetMemberExpression<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expr)
    {
        var lambda = expr.Body;
        
        if (lambda is UnaryExpression ue)
        {
            lambda = ue.Operand;
        }

        if (lambda is MemberExpression memberExpression)
        {
            return memberExpression;
        }

        throw new ArgumentException($"Cannot get member expression from '{expr}'.");
    }
}