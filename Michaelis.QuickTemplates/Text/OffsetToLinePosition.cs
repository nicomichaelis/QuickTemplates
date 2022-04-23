using System;
using System.Collections.Generic;

namespace Michaelis.QuickTemplates.Text;

public class OffsetToLinePosition
{
    TextLocation BaseLocation { get; }
    List<int> breakOffsets = new List<int>();
    List<(int line, int col)> breakPositions = new List<(int line, int col)>();
    int length;

    public OffsetToLinePosition(ReadOnlySpan<char> targetString, string sourceLocation, int line = 1, int col = 1)
    {
        if (null == targetString) throw new ArgumentNullException(nameof(targetString));
        if (null == sourceLocation) throw new ArgumentNullException(nameof(sourceLocation));

        BaseLocation = new(sourceLocation, 0, 0);

        if (!BaseTemplate.IsNewlineAtIndex(targetString, 0))
        {
            breakOffsets.Add(0);
            breakPositions.Add((line, col));
        }

        int i = 0;
        while (i < targetString.Length)
        {
            if (BaseTemplate.IsNewlineAtIndex(targetString, i, out int len))
            {
                i += len;
                breakOffsets.Add(i);
                breakPositions.Add((++line, 1));
            }
            else
                i++;
        }
        length = targetString.Length;
    }

    public TextLocation GetPosition(int offset)
    {
        if (offset < 0 || offset > length) throw new ArgumentOutOfRangeException(nameof(offset));

        var index = breakOffsets.BinarySearch(offset);
        if (index < 0)
        {
            index = ~index - 1;
            var lastBreakPos = breakPositions[index];
            var colOffset = offset - breakOffsets[index];
            return BaseLocation with { Line = lastBreakPos.line, Col = lastBreakPos.col + colOffset };
        }
        else
        {
            return BaseLocation with { Line = breakPositions[index].line, Col = breakPositions[index].col };
        }
    }
}
