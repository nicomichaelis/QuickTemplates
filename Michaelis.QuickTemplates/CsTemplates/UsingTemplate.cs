#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\UsingTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\UsingTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\UsingTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\UsingTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class UsingTemplate : CsBaseTemplate
    {

        public void TransformText(TemplateSelector selector, UsingNode use)
        {
            if (null == selector) throw new ArgumentNullException(nameof(selector));
            if (null == use) throw new ArgumentNullException(nameof(use));

#line hidden
            WriteNoBreakIndent("using ");
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\UsingTemplate.tt"
       $"{(use.Namespace)}");
#line hidden
            WriteNoBreakIndent(";");
            WriteLine();
        }
#line default
    }

}
