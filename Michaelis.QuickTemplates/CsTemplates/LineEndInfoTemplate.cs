#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class LineEndInfoTemplate : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        LineEndInfoNode lineEndInfoNode
        )
    {
#line hidden
#line 7 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
   SkipIndent();
#line hidden
        WriteNoBreakIndent("#line ");
        WriteFormated(
#line 7 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineEndInfoTemplate.tt"
                         $"{(lineEndInfoNode.Mode.ToString().ToLower())}");
#line hidden
        WriteLine();
#line default

    }
}
