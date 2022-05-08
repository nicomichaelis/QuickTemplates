#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class LineInfoTemplate : CsBaseTemplate
    {

        public void TransformText(TemplateSelector selector, LineInfoNode lineInfoNode)
        {
            if (null == selector) throw new ArgumentNullException(nameof(selector));
            if (null == lineInfoNode) throw new ArgumentNullException(nameof(lineInfoNode));

#line hidden
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
   SkipIndent();
#line hidden
            WriteNoBreakIndent("#line ");
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
                          $"{(lineInfoNode.Line)}");
#line hidden
            WriteNoBreakIndent(" \"");
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\LineInfoTemplate.tt"
                                                    $"{(lineInfoNode.Filename)}");
#line hidden
            WriteNoBreakIndent("\"");
            WriteLine();
        }
#line default
    }

}
