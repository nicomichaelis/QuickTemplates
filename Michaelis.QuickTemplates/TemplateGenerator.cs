using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

internal class TemplateGenerator
{
    public TemplateGenerator(IInputReader inputs, IResultWriter writer, DiagnosticsCollection diagnostics)
    {
        InputReader = inputs ?? throw new ArgumentNullException(nameof(inputs));
        ResultWriter = writer ?? throw new ArgumentNullException(nameof(writer)); ;
        Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
    }

    IInputReader InputReader { get; }
    DiagnosticsCollection Diagnostics { get; }
    IResultWriter ResultWriter { get; }

    public async Task<int> Run(CancellationToken cancel)
    {
        var metaData = await ProcessMeta(cancel);
        if (Diagnostics.ContainsErrors) return -1;

        return 0;
    }

    private async Task<TemplateDirectiveAndMetaData[]> ProcessMeta(CancellationToken cancel)
    {
        async Task<TemplateDirectiveAndMetaData> GetTemplateDirectiveData(Task<InputData> inputTask)
        {
            var input = await inputTask;
            cancel.ThrowIfCancellationRequested();
            TemplateDirectiveReader directiveReader = new(input.SourceText, input.SourceName);
            var directives = directiveReader.ProcessTemplate();

            cancel.ThrowIfCancellationRequested();
            MetaReader metaReader = new();
            var meta = metaReader.DecodeMeta(directives, Diagnostics);

            return new TemplateDirectiveAndMetaData(input.SourceName, directives, meta);
        }

        try
        {
            return await Task.WhenAll(InputReader.GetInputs(cancel).Select(z => GetTemplateDirectiveData(z)));
        }
        catch (Exception e)
        {
            Diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, DiagnosticMessages.Exception(e, null)));
            return null;
        }
    }

    record TemplateDirectiveAndMetaData(string SourceName, List<TemplateDirective> Directives, List<MetaData> Meta);
}

