#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ContextClassCodeTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ContextClassCodeTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ContextClassCodeTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ContextClassCodeTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class ContextClassCodeTemplate : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        ContextClassCodeNode cccn
        )
    {
#line hidden
        WriteNoBreakIndent("public global::System.IO.TextWriter Writer { get; init; }");
        WriteLine();
        WriteNoBreakIndent("public bool EoL { get; set; } = true;");
        WriteLine();
        WriteNoBreakIndent("public global::System.Collections.Generic.Stack<string> Indents { get; } = new global::System.Collections.Generic.Stack<string>();");
        WriteLine();
        WriteNoBreakIndent("public global::System.IFormatProvider FormatProvider { get; set; }");
        WriteLine();
#line default

    }
}
