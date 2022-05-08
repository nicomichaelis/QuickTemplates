#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class FixedLineTemplate : CsBaseTemplate
    {

        public void TransformText(TemplateSelector selector, FixedLineNode fixedLineNode)
        {
            if (null == selector) throw new ArgumentNullException(nameof(selector));
            if (null == fixedLineNode) throw new ArgumentNullException(nameof(fixedLineNode));

#line hidden
#line 9 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
    if (!fixedLineNode.Indented) SkipIndent();
#line hidden
            WriteFormated(
#line 10 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\FixedLineTemplate.tt"
   $"{(fixedLineNode.Content)}");
#line hidden
            WriteLine();
        }
#line default
    }

}
