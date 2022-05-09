#line 5 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
using Michaelis.QuickTemplates.Meta;
#line 2 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
using System;
#line 3 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
using System.Collections.Generic;
#line 4 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
using System.Linq;
#line default
#nullable disable

namespace Michaelis.QuickTemplates
{
    internal partial class TemplateFile : BaseTemplate
    {

        private void GenerateMethodTemplate(List<TemplateDirective> directives, Template template)
        {
            if (null == directives) throw new ArgumentNullException(nameof(directives));
            if (null == template) throw new ArgumentNullException(nameof(template));

#line hidden
#line 10 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
            foreach (var dir in directives.Where(z => z.Mode == DirectiveMode.ClassCode))
            {
                ApplyDirective(dir, template, FinishLineInfoMode.Default);
            }
#line hidden
            WriteLine();
            WriteFormated(
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
 $"{(template.TransformMethodVisibility != "" ? template.TransformMethodVisibility + " " : "")}");
#line hidden
            WriteFormated(
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
                                                                                                $"{(template.TransformMethodAttribute != null ? template.TransformMethodAttribute + " " : "")}");
#line hidden
            WriteNoBreakIndent("void ");
            WriteFormated(
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
                                                                                                                                                                                                    $"{(template.TransformMethod)}");
#line hidden
            WriteNoBreakIndent("(");
#line 15 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
                                                                                                                                                                                                                                      PrintMethodParameters(Meta);
#line hidden
            WriteNoBreakIndent(")");
            WriteLine();
            WriteNoBreakIndent("{");
            WriteLine();
#line 18 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
            PushIndent("    ");
            foreach (var param in Meta.OfType<Parameter>().Where(z => z.Required))
            {
                if (param.Availability == ParameterAvailability.Method) {
                    WriteLine($"if (null == {param.Name}) throw new ArgumentNullException(nameof({param.Name}));");
                }
                else {
                    WriteLine($"if (!_priv_set_{param.Name}) throw new InvalidOperationException($\"{{nameof({param.Name})}} was not set\");");
                }
            }
#line hidden
            WriteLine();
#line 30 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
            if (template.Linepragmas)
            {
                SkipIndent();
#line hidden
            WriteNoBreakIndent("#line hidden");
            WriteLine();
#line 34 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
            }

            foreach (var dir in directives.Where(z => z.Mode != DirectiveMode.ClassCode))
            {
                ApplyDirective(dir, template, FinishLineInfoMode.Hidden);
            }
            PopIndent();
#line hidden
            WriteNoBreakIndent("}");
            WriteLine();
#line 42 "D:\ws\QuickTemplates\Michaelis.QuickTemplates\Templates\GenerateTemplateMethod.tt"
   SkipIndent();
#line hidden
            WriteNoBreakIndent("#line default");
            WriteLine();
        }
#line default
    }

}
