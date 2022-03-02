using System;
using System.Linq;
using Microsoft.VisualBasic;

namespace ToDo.Service.Tests.Extensions;

internal static class ReflectionExtensions
{
    public static string ShortName(this Type type)
    {
        var name = type.Name;
        var args = Array.Empty<string>();
        
        if (type is {IsGenericType: true})
        {
            args = type.GenericTypeArguments.Select(t => t.ShortName()).ToArray();
        }

        return name.Replace($"`{args.Length}", $"<{string.Join(", ", args)}>");
    }
}