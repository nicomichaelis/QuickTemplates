#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class ClassTemplate : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        ClassNode cls
        )
    {
#line hidden
        WriteLine();
#line 10 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    foreach (var headNode in cls.Head)
    {
        ApplyNode(Context, selector, headNode);
    }
#line hidden
        WriteFormated(
#line 14 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
  $"{(cls.Modifier)}");
#line hidden
        WriteFormated(
#line 14 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                     $"{(GetIf(" ", cls.Modifier))}");
#line hidden
        WriteNoBreakIndent("partial class ");
        WriteFormated(
#line 14 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                  $"{(cls.Classname)}");
#line hidden
        WriteFormated(
#line 14 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                                      $"{(GetIf(" : ", cls.InheritsFrom))}");
#line hidden
        WriteFormated(
#line 14 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                                                                           $"{(cls.InheritsFrom)}");
#line hidden
        WriteLine();
        WriteNoBreakIndent("{");
        WriteLine();
#line 16 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    PushIndent("    ");
    foreach (var contentNode in cls.Content)
    {
        ApplyNode(Context, selector, contentNode);
    }
    PopIndent();
#line hidden
        WriteNoBreakIndent("}");
        WriteLine();
#line 24 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    foreach (var bottomNode in cls.Bottom)
    {
        ApplyNode(Context, selector, bottomNode);
    }
#line hidden
#line default

    }
}
