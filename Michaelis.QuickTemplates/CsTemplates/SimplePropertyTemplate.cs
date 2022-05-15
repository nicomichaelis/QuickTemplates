#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
using System;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates.CsTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class SimplePropertyTemplate : CsBaseTemplate
    {

        public void TransformText(TemplateSelector selector, SimplePropertyNode propertyNode)
        {
            if (null == selector) throw new ArgumentNullException(nameof(selector));
            if (null == propertyNode) throw new ArgumentNullException(nameof(propertyNode));

#line hidden
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
 $"{(propertyNode.Modifier)}");
#line hidden
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                             $"{(GetIf(" ", propertyNode.Modifier))}");
#line hidden
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                                                                     $"{(propertyNode.PropertyType)}");
#line hidden
            WriteNoBreakIndent(" ");
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                                                                                                      $"{(propertyNode.Propertyname)}");
#line hidden
            WriteNoBreakIndent(" { ");
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                                                                                                                                         $"{(propertyNode.GetAccessor)}");
#line hidden
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                                                                                                                                                                        $"{(GetIf(" ", propertyNode.GetAccessor, propertyNode.SetAccessor))}");
#line hidden
            WriteFormated(
#line 8 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\CsTemplates\SimplePropertyTemplate.tt"
                                                                                                                                                                                                                                             $"{(propertyNode.SetAccessor)}");
#line hidden
            WriteNoBreakIndent(" }");
            WriteLine();
        }
#line default
    }

}
