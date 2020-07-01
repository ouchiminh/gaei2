using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtension
{
    public static IEnumerator<T> FindMin<T, TValue>(this IEnumerable<T> self, Func<T, TValue> pred) where TValue : IComparable
    {
        var first = self.GetEnumerator();
        if (self.Count() == 0) return null;
        first.MoveNext();
        var min = pred(first.Current);
        var minE = first;
        while (first.MoveNext())
        {
            var tmp = pred(first.Current);
            if(min.CompareTo(tmp) > 0)
            {
                min = tmp;
                minE = first;
            }
        }
        return minE;
    }

    public static IEnumerator<T> FindMax<T, TValue>(this IEnumerable<T> self, Func<T, TValue> pred) where TValue : IComparable
    {
        var first = self.GetEnumerator();
        if (self.Count() == 0) return null;
        first.MoveNext();
        var max = pred(first.Current);
        var maxE = first;
        while (first.MoveNext())
        {
            var tmp = pred(first.Current);
            if(max.CompareTo(tmp) < 0)
            {
                max = tmp;
                maxE = first;
            }
        }
        return maxE;
    }

}
