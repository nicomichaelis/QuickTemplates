using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

partial class BaseTemplate
{
    internal void StartLineInfo(Template template, TemplateDirective directive)
    {
        if (template.Linepragmas)
        {
            SkipIndent();
            WriteLine($"#line {directive.Location.Line} \"{directive.Location.SourceName}\"");
        }
    }

    internal void FinishLineInfo(Template template, FinishLineInfoMode mode)
    {
        if (template.Linepragmas)
        {
            switch (mode)
            {
                case FinishLineInfoMode.Hidden:
                    SkipIndent();
                    WriteLine("#line hidden");
                    break;
                case FinishLineInfoMode.Default:
                    SkipIndent();
                    WriteLine("#line default");
                    break;
                case FinishLineInfoMode.None:
                default: break;
            }
        }
    }
}
