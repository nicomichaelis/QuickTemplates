#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class MethodTemplate : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        MethodNode methodNode
        )
    {
#line hidden
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
    foreach (var headNode in methodNode.Head)
    {
        ApplyNode(Context, selector, headNode);
    }
#line hidden
        WriteLine();
        WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
$"{(methodNode.Modifier)}");
#line hidden
        WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
                          $"{(GetIf(" ", methodNode.Modifier))}");
#line hidden
        WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
                                                                $"{(methodNode.ReturnType)}");
#line hidden
        WriteNoBreakIndent(" ");
        WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
                                                                                             $"{(methodNode.MethodName)}");
#line hidden
        WriteNoBreakIndent("(");
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
                                                                                                                             PushIndent("    ");
    foreach (var paramNode in methodNode.Parameters)
    {
       ApplyNode(Context, selector, paramNode);
    }
#line hidden
        WriteNoBreakIndent(")");
#line 18 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
      PopIndent();
#line hidden
        WriteLine();
        WriteNoBreakIndent("{");
        WriteLine();
#line 20 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\MethodTemplate.tt"
    PushIndent("    ");
    foreach (var contentNode in methodNode.Content)
    {
       ApplyNode(Context, selector, contentNode);
    }
    PopIndent();
#line hidden
        WriteLine();
        WriteNoBreakIndent("}");
        WriteLine();
#line default

    }
}
