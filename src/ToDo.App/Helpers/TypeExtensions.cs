using System;

namespace ToDo.App.Helpers;

public static class TypeExtensions
{
    public static bool IsDate(this Type type)
    {
        return Type.GetTypeCode(type) == TypeCode.DateTime;
    }
    public static bool IsBoolean(this Type type)
    {
        return Type.GetTypeCode(type) == TypeCode.Boolean;
    }
    
    public static bool IsNumeric(this Type type)
    {
        return Type.GetTypeCode(type) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Single => true,
            _ => false
        };
    }
}