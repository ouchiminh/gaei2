using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtension
{
    public static T FindMin<T, TValue>(this IEnumerable<T> self, Func<T, TValue> pred) where TValue : IComparable
    {
        var first = self.GetEnumerator();
        if (self.Count() == 0) throw new IndexOutOfRangeException();
        first.MoveNext();
        var min = pred(first.Current);
        var minE = first.Current;
        while (first.MoveNext())
        {
            var tmp = pred(first.Current);
            if(min.CompareTo(tmp) > 0)
            {
                min = tmp;
                minE = first.Current;
            }
        }
        return minE;
    }

    public static T FindMax<T, TValue>(this IEnumerable<T> self, Func<T, TValue> pred) where TValue : IComparable
    {
        var first = self.GetEnumerator();
        if (self.Count() == 0) throw new IndexOutOfRangeException();
        first.MoveNext();
        var max = pred(first.Current);
        var maxE = first.Current;
        while (first.MoveNext())
        {
            var tmp = pred(first.Current);
            if(max.CompareTo(tmp) < 0)
            {
                max = tmp;
                maxE = first.Current;
            }
        }
        return maxE;
    }

}
