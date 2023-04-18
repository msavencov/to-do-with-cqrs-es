using System;
using System.Security;
using Google.Protobuf.WellKnownTypes;
using ToDo.Api.Contract.Shared;
using Type = System.Type;

namespace ToDo.Service.Helpers;

public static class ProtobufTypeHelpers
{
    public static object ToDate(this Timestamp timestamp, Type type)
    {
        if (type == typeof(DateTime))
        {
            return timestamp.ToDateTime();
        }

        if (type == typeof(DateTimeOffset))
        {
            return timestamp.ToDateTimeOffset();
        }

        throw new NotSupportedException($"Cannot convert the '{typeof(Timestamp)}' to '{type}'.");
    }
}