using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringMisc
{
    /// <summary>
    /// Same as String.EndsWith but does not allocate anything on the managed heap.
    /// </summary>
    /// <returns><c>true</c>, if with non alloc was endsed, <c>false</c> otherwise.</returns>
    /// <param name="a">The alpha component.</param>
    /// <param name="b">The blue component.</param>
    public static bool EndsWithNonAlloc(this string a, string b)
    {
        int ap = a.Length - 1;
        int bp = b.Length - 1;

        while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
        {
            ap--;
            bp--;
        }
        return (bp < 0 && a.Length >= b.Length) ||

                (ap < 0 && b.Length >= a.Length);
    }

    /// <summary>
    /// Same as String.StartsWith but does not allocate anything on the managed heap.
    /// </summary>
    /// <returns><c>true</c>, if with non alloc was startsed, <c>false</c> otherwise.</returns>
    /// <param name="a">The alpha component.</param>
    /// <param name="b">The blue component.</param>
    public static bool StartsWithNonAlloc(this string a, string b)
    {
        int aLen = a.Length;
        int bLen = b.Length;
        int ap = 0; int bp = 0;

        while (ap < aLen && bp < bLen && a[ap] == b[bp])
        {
            ap++;
            bp++;
        }
        return (bp == bLen && aLen >= bLen) ||

                (ap == aLen && bLen >= aLen);
    }

    /// <summary>
    /// 比较字符串是否相等，StringComparison.Ordinal
    /// </summary>
    /// <returns><c>true</c>, if ordinal was equalsed, <c>false</c> otherwise.</returns>
    /// <param name="a">The alpha component.</param>
    /// <param name="b">The blue component.</param>
    public static bool EqualsOrdinal(this string a,string b)
    {
        return a.Equals(b, System.StringComparison.Ordinal);
    }
}
