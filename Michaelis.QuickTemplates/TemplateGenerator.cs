using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Michaelis.QuickTemplates.CsTemplates;
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

    record TemplateDirectiveAndMetaData(InputData Input, List<TemplateDirective> Directives, List<MetaData> Meta);
    record ModelData(Dictionary<TemplateDirectiveAndMetaData, FileNode> Model);

    public async Task<int> Run(CancellationToken cancel)
    {
        var metaData = await ProcessMeta(cancel);
        if (Diagnostics.ContainsErrors) return -1;
        var modelData = await ProcessModel(metaData, cancel);
        if (Diagnostics.ContainsErrors) return -1;
        if (cancel.IsCancellationRequested) return -1;
        var outputData = GenerateOutputData(modelData);
        if (Diagnostics.ContainsErrors) return -1;
        await ResultWriter.Write(outputData);
        return 0;
    }

    async Task<TemplateDirectiveAndMetaData[]> ProcessMeta(CancellationToken cancel)
    {
        async Task<TemplateDirectiveAndMetaData> GetTemplateDirectiveData(Task<InputData> inputTask)
        {
            var input = await inputTask;
            cancel.ThrowIfCancellationRequested();
            TemplateDirectiveReader directiveReader = new(input);
            var directives = directiveReader.ProcessTemplate();

            cancel.ThrowIfCancellationRequested();
            MetaReader metaReader = new();
            var meta = metaReader.DecodeMeta(directives, Diagnostics);

            return new TemplateDirectiveAndMetaData(input, directives, meta);
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

    async Task<ModelData> ProcessModel(TemplateDirectiveAndMetaData[] metaData, CancellationToken cancel)
    {
        var data = await Task.WhenAll(metaData.Select(z => ProcessModelItem(z, cancel)));
        return new ModelData(new(data));

        static async Task<KeyValuePair<TemplateDirectiveAndMetaData, FileNode>> ProcessModelItem(TemplateDirectiveAndMetaData data, CancellationToken cancel)
        {
            await Task.Yield();
            cancel.ThrowIfCancellationRequested();
            ModelGenerator modelGenerator = new();
            var file = modelGenerator.Generate(data.Input, data.Meta, data.Directives);
            return new KeyValuePair<TemplateDirectiveAndMetaData, FileNode>(data, file);
        }
    }

    List<OutputData> GenerateOutputData(ModelData modelData)
    {
        var selector = new TemplateSelector(10); // TODO let the user decide which language version
        return modelData.Model.Select(z => CreateOutputData(z.Value, selector)).ToList();

        static OutputData CreateOutputData(FileNode node, TemplateSelector selector)
        {
            var transformText = selector.GetTemplateTransformForNode(node);
            var context = new CsBaseTemplateContext();
            return new OutputData(node.Filename, (writer) =>
            {
                context.Writer = writer;
                transformText(context);
            });
        }
    }
}

