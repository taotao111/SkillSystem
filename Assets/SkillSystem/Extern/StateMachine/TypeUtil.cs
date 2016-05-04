using System;
using System.Collections.Generic;

public static class TypeUtil
{
    public static string ToKey<T>()
    {
        return ToKey(typeof(T));
    }

    public static string ToKey(Type type)
    {
        return type.Name;
    }
}