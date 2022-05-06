using System;
using System.Collections.Generic;

namespace Michaelis.QuickTemplates;

public class NaturalSortComparer : Comparer<string>
{
    public override int Compare(string x, string y)
    {
        if (ReferenceEquals(x, y)) return 0;

        ReadOnlySpan<char>
            sx = x.AsSpan(),
            sy = y.AsSpan();

        int ix = 0,
            iy = 0;

        int result = 0;
        while (result == 0)
        {
            while (ix < sx.Length && Char.IsWhiteSpace(sx[ix])) ix++;
            while (iy < sy.Length && Char.IsWhiteSpace(sy[iy])) iy++;
            if (ix >= sx.Length)
            {
                result = iy >= sy.Length ? 0 : -1;
                break;
            }
            else if (iy >= sy.Length)
            {
                result = 1;
            }
            else
            {
                if (char.IsDigit(sx[ix]) && char.IsDigit(sy[ix]))
                {
                    (result, ix, iy) = CompareInt(sx, ix, sy, iy);
                }
                else
                {
                    result = sx[ix].CompareTo(sy[iy]);
                    ix++;
                    iy++;
                }
            }
        }
        return result;
    }

    (int result, int ix, int iy) CompareInt(ReadOnlySpan<char> sx, int ix, ReadOnlySpan<char> sy, int iy)
    {
        // skip trailing zeros
        var inix = sx[ix];
        if (System.Globalization.CharUnicodeInfo.GetNumericValue(inix) == 0.0)
        {
            while (ix < sx.Length && sx[ix] == inix) ix++;
        }
        var iniy = sy[iy];
        if (System.Globalization.CharUnicodeInfo.GetNumericValue(iniy) == 0.0)
        {
            while (iy < sy.Length && sy[iy] == iniy) iy++;
        }

        if (ix >= sx.Length || iy >= sy.Length) return (inix == iniy ? 0 : inix.CompareTo(iniy), ix, iy); // oops. only zeros

        var lx = ix;
        while (lx < sx.Length && char.IsDigit(sx[lx])) lx++;
        var ly = iy;
        while (ly < sy.Length && char.IsDigit(sy[ly])) ly++;

        if (lx != ly) return (lx.CompareTo(ly), lx, ly);
        var result = 0;
        while (result == 0 && ix < lx && iy < ly)
        {
            result = sx[ix].CompareTo(sy[iy]);
            ix++;
            iy++;
        }
        return (result, ix, iy);
    }
}
