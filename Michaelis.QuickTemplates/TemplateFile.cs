using System;
using System.Collections.Generic;
using System.Linq;
using Michaelis.QuickTemplates.Meta;
using Michaelis.QuickTemplates.Text;

namespace Michaelis.QuickTemplates;

partial class TemplateFile
{
    private void PrintMemberParameters(List<MetaData> meta)
    {
        var memberParameters = meta.OfType<Parameter>().Where(z => z.Availability == ParameterAvailability.Class).ToList();
        if (memberParameters.Any())
        {
            foreach (var z in memberParameters)
            {
                if (z.Required)
                {
                    Write($"private {z.Type} _priv_{z.Name} {{ get; set; }}");
                    if (!string.IsNullOrEmpty(z.Initializer))
                    {
                        Write($" = {z.Initializer};");
                    }
                    WriteLine();
                    Write($"private bool _priv_set_{z.Name} {{ get; set; }}");
                    if (!string.IsNullOrEmpty(z.Initializer))
                    {
                        Write($" = true;"); // this is quite stupid, but an initializer was requested and the property is required
                    }
                    WriteLine();
                    WriteLine($"public {z.Type} {z.Name}");
                    WriteLine($"{{");
                    WriteLine($"    get {{ return _priv_{z.Name}; }}");
                    WriteLine($"    set {{ _priv_{z.Name} = value; _priv_set_{z.Name} = true; }}");
                    WriteLine($"}}");
                }
                else
                {
                    Write($"public {z.Type} {z.Name} {{ get; set; }}");
                    if (!string.IsNullOrEmpty(z.Initializer))
                    {
                        Write($" = {z.Initializer};");
                    }
                    WriteLine();
                }
                WriteLine();
            }
        }
    }

    private bool IsInherited(Template template)
    {
        return !string.IsNullOrEmpty(template.Inherits);
    }

    private string InheritsFrom(Template template)
    {
        var inheritedBase = template.Inherits;
        return inheritedBase ?? (ClassName + "Base");
    }

    private void ApplyDirective(TemplateDirective dir, Template template, FinishLineInfoMode mode)
    {
        switch (dir.Mode)
        {
            case DirectiveMode.Meta:
                break;
            case DirectiveMode.Text:
                WriteLine($"WriteNoBreakIndent({StringUtils.SafeEncodeString(dir.Data)});");
                break;
            case DirectiveMode.Insert:
                WriteLine($"WriteFormated(");
                StartLineInfo(template, dir);
                if (template.Linepragmas)
                {
                    SkipIndent();
                    Write(new string(' ', Math.Max(0, dir.Location.Col - 1 - 3)));
                }
                else
                {
                    Write("    ");
                }
                WriteLine($"$\"{{({dir.Data})}}\");");
                FinishLineInfo(template, mode);
                break;
            case DirectiveMode.ClassCode:
            case DirectiveMode.Code:
                StartLineInfo(template, dir);
                if (template.Linepragmas)
                {
                    SkipIndent();
                    Write(new string(' ', Math.Max(0, dir.Location.Col - 1)));
                }
                else
                {
                    Write("    ");
                }
                WriteNoBreakIndent(dir.Data);
                WriteLine();
                FinishLineInfo(template, mode);
                break;
            case DirectiveMode.NewLine:
                WriteLine("WriteLine();");
                break;
            default: throw new NotImplementedException($"{dir.Mode}");
        }
    }

    private void PrintMethodParameters(List<MetaData> meta)
    {
        Write(string.Join(", ", meta.OfType<Parameter>().Where(z => z.Availability == ParameterAvailability.Method).Select(z => $"{z.Type} {z.Name}{(z.Initializer != null ? " " + z.Initializer : "")}")));
    }
}
