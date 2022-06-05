using System;
using System.Collections.Generic;
using Michaelis.QuickTemplates.CsTemplates;

namespace Michaelis.QuickTemplates.Text;

public class OffsetToLinePosition
{
    TextLocation BaseLocation { get; }
    List<int> _breakOffsets = new List<int>();
    List<(int line, int col)> _breakPositions = new List<(int line, int col)>();
    int _length;

    public OffsetToLinePosition(ReadOnlySpan<char> targetString, string sourceLocation, int line = 1, int col = 1)
    {
        if (null == targetString) throw new ArgumentNullException(nameof(targetString));
        if (null == sourceLocation) throw new ArgumentNullException(nameof(sourceLocation));

        BaseLocation = new(sourceLocation, 0, 0);

        if (!CsBaseTemplateBase.IsNewlineAtIndex(targetString, 0))
        {
            _breakOffsets.Add(0);
            _breakPositions.Add((line, col));
        }

        int i = 0;
        while (i < targetString.Length)
        {
            if (CsBaseTemplateBase.IsNewlineAtIndex(targetString, i, out int len))
            {
                i += len;
                _breakOffsets.Add(i);
                _breakPositions.Add((++line, 1));
            }
            else
                i++;
        }
        _length = targetString.Length;
    }

    public TextLocation GetPosition(int offset)
    {
        if (offset < 0 || offset > _length) throw new ArgumentOutOfRangeException(nameof(offset));

        var index = _breakOffsets.BinarySearch(offset);
        if (index < 0)
        {
            index = ~index - 1;
            var lastBreakPos = _breakPositions[index];
            var colOffset = offset - _breakOffsets[index];
            return BaseLocation with { Line = lastBreakPos.line, Col = lastBreakPos.col + colOffset };
        }
        else
        {
            return BaseLocation with { Line = _breakPositions[index].line, Col = _breakPositions[index].col };
        }
    }
}
