#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class ClassTemplate : CsBaseTemplate
    {

        public void TransformText(TemplateSelector selector, ClassNode cls)
        {
            if (null == selector) throw new ArgumentNullException(nameof(selector));
            if (null == cls) throw new ArgumentNullException(nameof(cls));

#line hidden
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    foreach (var headNode in cls.Head)
    {
        ApplyNode(Context, selector, headNode);
    }
#line hidden
            WriteLine();
            WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
 $"{(cls.Modifier)}");
#line hidden
            WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                    $"{(GetIf(" ", cls.Modifier))}");
#line hidden
            WriteNoBreakIndent("partial class ");
            WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                 $"{(cls.Classname)}");
#line hidden
            WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                                     $"{(GetIf(" : ", cls.InheritsFrom))}");
#line hidden
            WriteFormated(
#line 13 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
                                                                                                                          $"{(cls.InheritsFrom)}");
#line hidden
            WriteLine();
            WriteNoBreakIndent("{");
            WriteLine();
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    PushIndent("    ");
    foreach (var contentNode in cls.Content)
    {
        ApplyNode(Context, selector, contentNode);
    }
    PopIndent();
#line hidden
            WriteNoBreakIndent("}");
            WriteLine();
#line 23 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ClassTemplate.tt"
    foreach (var bottomNode in cls.Bottom)
    {
        ApplyNode(Context, selector, bottomNode);
    }
#line hidden
        }
#line default
    }

}
