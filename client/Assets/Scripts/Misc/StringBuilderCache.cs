using UnityEngine;
using System.Collections;
using System.Text;

public static class StringBuilderCache
{
    private static StringBuilder _cache = new StringBuilder();
    private const int MAX_BUILDER_SIZE = 512;

    public static void Clear(this StringBuilder sb)
    {
        sb.Length = 0;
    }

    public static StringBuilder Acquire(int capacity = 256)
    {
        StringBuilder cache = StringBuilderCache._cache;
        if (cache != null && cache.Capacity >= capacity)
        {
            StringBuilderCache._cache = null;
            cache.Clear();
            return cache;
        }
        return new StringBuilder(capacity);
    }

    public static string GetStringAndRelease(StringBuilder sb)
    {
        string arg_0C_0 = sb.ToString();
        StringBuilderCache.Release(sb);
        return arg_0C_0;
    }

    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= 512)
        {
            StringBuilderCache._cache = sb;
        }
    }
}
