using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzSharp.Utils;

public static class ListResize
{
    public static void Resize<T>(this List<T> list, int sz, T c = default(T)!)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }
}
