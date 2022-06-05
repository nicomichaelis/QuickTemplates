using System;
using System.Collections.Generic;

namespace Michaelis.QuickTemplates;

public enum InterspersePosition { Init, Between, End };

public static class LinqUtils
{
    public static IEnumerable<TResult> Intersperse<TResult>(this IEnumerable<TResult> source, Func<InterspersePosition, (bool, TResult)> selector)
    {
        using (var enu = source.GetEnumerator())
        {
            if (enu.MoveNext())
            {
                (bool emit, TResult item) = selector(InterspersePosition.Init);
                if (emit)
                {
                    yield return item;
                }

                bool hasCurrent = true;
                do
                {
                    yield return enu.Current;
                    hasCurrent = enu.MoveNext();
                    (emit, item) = selector(hasCurrent ? InterspersePosition.Between : InterspersePosition.End);
                    if (emit)
                    {
                        yield return item;
                    }
                } while (hasCurrent);
            }
        }
    }
}