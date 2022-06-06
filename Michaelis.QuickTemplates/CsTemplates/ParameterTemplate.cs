#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates;

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
internal partial class ParameterTemplate : CsBaseTemplate
{

    public void TransformText(
        TemplateSelector selector,
        ParameterNode parameterNode
        )
    {
#line hidden
        WriteFormated(
#line 7 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
$"{(parameterNode.ParameterType)}");
#line hidden
        WriteNoBreakIndent(" ");
        WriteFormated(
#line 7 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\ParameterTemplate.tt"
                                   $"{(parameterNode.ParameterName)}");
#line hidden
#line default

    }
}
