
namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class CsBaseTemplateContext
{
    public global::System.IO.TextWriter Writer { get; init; }
    public bool EoL { get; set; } = true;
    public global::System.Collections.Generic.Stack<string> Indents { get; } = new global::System.Collections.Generic.Stack<string>();
    public global::System.IFormatProvider FormatProvider { get; set; }
}
