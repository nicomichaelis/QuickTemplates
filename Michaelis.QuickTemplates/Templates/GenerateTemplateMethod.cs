#line 5 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
using Michaelis.QuickTemplates.Meta;
#line 2 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
using System;
#line 3 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
using System.Collections.Generic;
#line 4 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates
{
    internal partial class TemplateFile : BaseTemplate
    {

        private void GenerateMethodTemplate(List<TemplateDirective> directives, Template template)
        {
            if (null == directives) throw new ArgumentNullException(nameof(directives));
            if (null == template) throw new ArgumentNullException(nameof(template));

#line hidden
#line 9 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
            foreach (var dir in directives.Where(z => z.Mode == DirectiveMode.ClassCode))
            {
                ApplyDirective(dir, template, FinishLineInfoMode.Default);
            }
#line hidden
            WriteLine();
            WriteFormated(
#line 14 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
 $"{(template.TransformMethodVisibility != TemplateVisibility.none ? template.TransformMethodVisibility.ToString() + " " : "")}");
#line hidden
            WriteFormated(
#line 14 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
                                                                                                                                $"{(template.TransformMethodAttribute != null ? template.TransformMethodAttribute + " " : "")}");
#line hidden
            WriteNoBreakIndent("void ");
            WriteFormated(
#line 14 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
                                                                                                                                                                                                                                    $"{(template.TransformMethod)}");
#line hidden
            WriteNoBreakIndent("(");
#line 14 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
                                                                                                                                                                                                                                                                      PrintMethodParameters(Meta);
#line hidden
            WriteNoBreakIndent(")");
            WriteLine();
            WriteNoBreakIndent("{");
            WriteLine();
#line 17 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
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
#line 29 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
            if (template.Linepragmas) 
            {
                SkipIndent();
#line hidden
            WriteNoBreakIndent("#line hidden");
            WriteLine();
#line 33 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
            }

            foreach (var dir in directives.Where(z => z.Mode != DirectiveMode.ClassCode))
            {
                ApplyDirective(dir, template, FinishLineInfoMode.Hidden);
            }
            PopIndent();
#line hidden
            WriteNoBreakIndent("}");
            WriteLine();
#line 41 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\GenerateTemplateMethod.tt"
   SkipIndent();
#line hidden
            WriteNoBreakIndent("#line default");
            WriteLine();
        }
#line default
    }

}