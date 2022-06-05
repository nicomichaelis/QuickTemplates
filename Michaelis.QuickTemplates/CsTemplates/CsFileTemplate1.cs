#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class CsFileTemplate1 : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        FileNode file
        )
    {
#line hidden
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
    foreach (var headNode in file.Head)
    {
        ApplyNode(Context, selector, headNode);
    }
#line hidden
        WriteLine();
        WriteNoBreakIndent("namespace ");
        WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
          $"{(file.Namespace)}");
#line hidden
        WriteLine();
        WriteNoBreakIndent("{");
        WriteLine();
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
    PushIndent("    ");
    foreach (var contentNode in file.Content)
    {
        ApplyNode(Context, selector, contentNode);
    }
    PopIndent();
#line hidden
        WriteNoBreakIndent("}");
        WriteLine();
#line 23 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\CsFileTemplate1.tt"
    foreach (var bottomNode in file.Bottom)
    {
        ApplyNode(Context, selector, bottomNode);
    }
#line hidden
#line default

    }
}
