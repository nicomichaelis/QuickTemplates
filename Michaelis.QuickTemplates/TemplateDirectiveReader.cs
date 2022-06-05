using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Michaelis.QuickTemplates.CsTemplates;
using Michaelis.QuickTemplates.Text;

namespace Michaelis.QuickTemplates;

class TemplateDirectiveReader
{
    static readonly Regex _directive_pattern_regex = new Regex(@"\<#((?<META>@\s*(?<META_DATA>.*?)\s*#\>)|(?<INSERT>\=\s*(?<INSERT_EXPR>.*?)\s*#\>)|(?<CODE>(?<CODE_CLASS>\+)?\s*(?<CODE_TEXT>(?![#@])(\r|\n|.)*?)\s*#\>))", RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    string TemplateText { get; }
    string SourceFile { get; }

    public TemplateDirectiveReader(InputData input)
    {
        TemplateText = input.SourceText;
        SourceFile = input.FullName;
    }

    public List<TemplateDirective> ProcessTemplate()
    {
        var directives = new List<TemplateDirective>();

        ReadOnlySpan<char> textSpan = TemplateText.AsSpan();
        var m = _directive_pattern_regex.Match(TemplateText);
        var otlp = new OffsetToLinePosition(TemplateText, SourceFile);
        int offset = 0;
        while (m.Success && m.Groups["META"].Success && m.Index == offset)
        {
            offset = m.Index + m.Length;
            if (CsBaseTemplateBase.IsNewlineAtIndex(TemplateText, offset, out int len)) offset += len;
            directives.Add(ReadDirective(m, otlp));
            m = m.NextMatch();
        }

        while (m.Success)
        {
            ProcessSimpleText(offset, m.Index, textSpan, otlp, directives);
            offset = m.Index + m.Length;
            directives.Add(ReadDirective(m, otlp));
            m = m.NextMatch();
        }
        ProcessSimpleText(offset, textSpan.Length, textSpan, otlp, directives);

        return directives;
    }

    TemplateDirective ReadDirective(Match m, OffsetToLinePosition otlp)
    {
        if (!m.Success) throw new ArgumentOutOfRangeException(nameof(m));
        if (m.Groups["META"].Success)
        {
            return new TemplateDirective(DirectiveMode.Meta, m.Groups["META_DATA"].Value, otlp.GetPosition(m.Groups["META_DATA"].Index));
        }
        if (m.Groups["INSERT"].Success)
        {
            return new TemplateDirective(DirectiveMode.Insert, m.Groups["INSERT_EXPR"].Value, otlp.GetPosition(m.Groups["INSERT_EXPR"].Index));
        }
        if (m.Groups["CODE"].Success)
        {
            return new TemplateDirective(m.Groups["CODE_CLASS"].Success ? DirectiveMode.ClassCode : DirectiveMode.Code, m.Groups["CODE_TEXT"].Value, otlp.GetPosition(m.Groups["CODE_TEXT"].Index));
        }
        else
            throw new NotImplementedException();
    }

    void ProcessSimpleText(int offset, int end, ReadOnlySpan<char> templateText, OffsetToLinePosition otlp, List<TemplateDirective> directives)
    {
        int textStartOffset = offset;
        while (offset < end)
        {
            if (CsBaseTemplateBase.IsNewlineAtIndex(templateText, offset, out int len))
            {
                if (offset - textStartOffset > 0)
                {
                    directives.Add(new TemplateDirective(DirectiveMode.Text, templateText.Slice(textStartOffset, offset - textStartOffset).ToString(), otlp.GetPosition(textStartOffset)));
                }
                directives.Add(new TemplateDirective(DirectiveMode.NewLine, null, otlp.GetPosition(offset)));
                offset += len;
                textStartOffset = offset;
            }
            else
            {
                offset++;
            }
        }
        if (offset - textStartOffset > 0)
        {
            directives.Add(new TemplateDirective(DirectiveMode.Text, templateText.Slice(textStartOffset, offset - textStartOffset).ToString(), otlp.GetPosition(textStartOffset)));
        }
    }
}
