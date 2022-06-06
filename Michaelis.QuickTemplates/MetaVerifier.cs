using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

record MetaVerifierDataItem(InputData Input, List<TemplateDirective> Directives, List<MetaData> Meta);

class MetaVerifier
{
    internal Task Verify(IEnumerable<MetaVerifierDataItem> data, DiagnosticsCollection diagnostics)
    {
        Dictionary<string, (MetaVerifierDataItem item, Template template)> nameLookup = new();
        foreach (var item in data)
        {
            Verify(item, nameLookup, diagnostics);
        }
        foreach (var value in nameLookup.Values)
        {
            bool inherit = VerifyInheritance(value.item, value.template, nameLookup, diagnostics);
            VerifyParameterNames(value.item, value.template, nameLookup, inherit, diagnostics);
        }
        return Task.FromResult(true);
    }

    void Verify(MetaVerifierDataItem item, Dictionary<string, (MetaVerifierDataItem item, Template template)> nameLookup, DiagnosticsCollection diagnostics)
    {
        var (input, directives, meta) = item;
        var templateDirective = meta.OfType<Template>().LastOrDefault();
        if (templateDirective == null)
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.TemplateDirectiveMissing(input.FullName)));
            return;
        }

        if (string.IsNullOrEmpty(templateDirective.Namespace) || templateDirective.Namespace != templateDirective.Namespace.Trim())
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.RequiredPropertyMissing(templateDirective.Directive.Location, nameof(templateDirective.Namespace))));
        }

        string templateName = templateDirective.TemplateName;
        if (nameLookup.ContainsKey(templateName))
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.TemplateNameNotUnique(templateDirective.Directive.Location)));
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.TemplateNameNotUnique(nameLookup[templateName].template.Directive.Location)));
        }
        else
            nameLookup.Add(templateName, (item, templateDirective));

        foreach (var template in meta.OfType<Template>().Where(z => z != templateDirective))
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.MultipleTemplateDirectives(template.Directive.Location)));
        }

        foreach (var line in meta.OfType<Line>())
        {
            if (string.IsNullOrEmpty(line.Text) || line.Position == LinePostition.Undefined)
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.IncorrectParameter(line.Directive.Location)));
            }
        }
    }

    bool VerifyInheritance(MetaVerifierDataItem item, Template template, Dictionary<string, (MetaVerifierDataItem item, Template template)> nameLookup, DiagnosticsCollection diagnostics)
    {
        HashSet<string> nameStack = new();
        if (string.IsNullOrEmpty(template.Inherits)) return false;

        if (!nameLookup.ContainsKey(template.Inherits))
        {
            diagnostics.Add(new Diagnostic(DiagnosticSeverity.Warning, DiagnosticMessages.BaseNotFound(template.Directive.Location, template.Inherits)));
            return false;
        }

        var curtemplate = template;
        do
        {
            if (nameStack.Contains(curtemplate.TemplateName))
            {
                diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, DiagnosticMessages.InfiniteRecursion(curtemplate.Directive.Location)));
                return false;
            }
            nameStack.Add(curtemplate.TemplateName);
            if (nameLookup.TryGetValue(curtemplate.Inherits, out var follow))
            {
                curtemplate = follow.template;
            }
            else curtemplate = null;
        } while (curtemplate != null);
        return true;
    }

    void VerifyParameterNames(MetaVerifierDataItem item, Template template, Dictionary<string, (MetaVerifierDataItem item, Template template)> nameLookup, bool inherit, DiagnosticsCollection diagnostics)
    {
        HashSet<string> names = new HashSet<string>();

        if (inherit)
        {
            string inh = template.Inherits;
            while (nameLookup.TryGetValue(inh, out var pitem))
            {
                inh = pitem.template.Inherits;
                foreach (var pparam in pitem.item.Meta.OfType<Parameter>())
                {
                    if (!string.IsNullOrEmpty(pparam.Name))
                    {
                        names.Add(pparam.Name);
                    }
                }
            }
        }

        foreach (var param in item.Meta.OfType<Parameter>())
        {
            if (string.IsNullOrEmpty(param.Name) || string.IsNullOrEmpty(param.Type) || param.Name.Trim() != param.Name || param.Type.Trim() != param.Type)
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.IncorrectParameter(param.Directive.Location)));
            }
            else if (names.Contains(param.Name))
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.ParameterAlreadyInUse(param.Directive.Location)));
            }
            else
            {
                names.Add(param.Name);
            }
        }
    }
}
