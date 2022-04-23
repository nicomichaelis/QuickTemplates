using Michaelis.QuickTemplates.Text;

namespace Michaelis.QuickTemplates;

public class TemplateDirective
{
    public TemplateDirective(DirectiveMode mode, string data, TextLocation pos)
    {
        Mode = mode;
        Data = data;
        Location = pos;
    }
    public DirectiveMode Mode { get; }
    public string Data { get; }
    public TextLocation Location { get; }
}
