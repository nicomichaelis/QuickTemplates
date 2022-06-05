using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates
{
    class MetaVerifier
    {
        internal Task Verify(IEnumerable<(InputData Input, List<TemplateDirective> Directives, List<MetaData> Meta)> data, DiagnosticsCollection diagnostics)
        {
            foreach (var item in data)
            {
                Verify(item.Input, item.Directives, item.Meta, diagnostics);
            }
            return Task.FromResult(true);
        }

        private void Verify(InputData input, List<TemplateDirective> directives, List<MetaData> meta, DiagnosticsCollection diagnostics)
        {
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

            foreach (var template in meta.OfType<Template>().Where(z => z != templateDirective))
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.MultipleTemplateDirectives(template.Directive.Location)));
            }

            HashSet<string> names = new HashSet<string>();
            foreach (var param in meta.OfType<Parameter>())
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

            foreach (var line in meta.OfType<Line>())
            {
                if (string.IsNullOrEmpty(line.Text) || line.Position == LinePostition.Undefined)
                {
                    diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.IncorrectParameter(line.Directive.Location)));
                }
            }
        }
    }
}