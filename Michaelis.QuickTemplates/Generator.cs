using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

class Generator
{
    private List<FileInfo> Inputs;
    private Dictionary<FileInfo, List<TemplateDirective>> Directives;
    private Dictionary<FileInfo, List<MetaData>> Meta;
    private DiagnosticsCollection Diagnostics;

    public Generator(List<FileInfo> inputs, DiagnosticsCollection diagnostics)
    {
        Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
        Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
    }

    public async Task<bool> ReadDirectives(System.Threading.CancellationToken cancel)
    {
        Directives = new Dictionary<FileInfo, List<TemplateDirective>>();
        var tasks = Inputs.Select(input => Task.Factory.StartNew(() =>
            {
                cancel.ThrowIfCancellationRequested();
                return ReadFileDirectives(input);
            })).ToList();

        foreach (var task in await Task.WhenAll(tasks))
        {
            Directives.Add(task.file, task.directives);
        }

        return !Diagnostics.ContainsErrors;
    }

    private (FileInfo file, List<TemplateDirective> directives) ReadFileDirectives(FileInfo inFile)
    {
        try
        {
            string infileText = File.ReadAllText(inFile.FullName);
            var tdr = new TemplateDirectiveReader(infileText, inFile.FullName);
            return (inFile, tdr.ProcessTemplate());
        }
        catch (IOException ioex)
        {
            Diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, DiagnosticMessages.Exception(ioex, new Text.TextLocation(inFile.FullName, 1, 1))));
            return (inFile, new List<TemplateDirective>());
        }
    }

    public async Task<bool> ReadMeta()
    {
        Meta = new Dictionary<FileInfo, List<MetaData>>();
        var tasks = Inputs.Select(input => Task.Factory.StartNew(() => ReadFileMeta(input))).ToList();
        await Task.WhenAll(tasks);
        foreach (var task in tasks)
        {
            Meta.Add(task.Result.file, task.Result.metadata);
        }

        return !Diagnostics.ContainsErrors;
    }

    private (FileInfo file, List<MetaData> metadata) ReadFileMeta(FileInfo inFile)
    {
        MetaReader rdr = new MetaReader();
        var meta = rdr.DecodeMeta(Directives[inFile], Diagnostics);
        return (inFile, meta);
    }

    public bool VerifyMeta()
    {
        foreach (var input in Inputs)
        {
            var meta = Meta[input];
            var templateDirective = meta.OfType<Template>().LastOrDefault();
            if (templateDirective == null)
            {
                Diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.TemplateDirectiveMissing(input.FullName)));
                continue;
            }

            if (string.IsNullOrEmpty(templateDirective.Namespace) || templateDirective.Namespace != templateDirective.Namespace.Trim())
            {
                Diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.RequiredPropertyMissing(templateDirective.Directive.Location, nameof(templateDirective.Namespace))));
                continue;
            }

            foreach (var template in meta.OfType<Template>().Where(z => z != templateDirective))
            {
                Diagnostics.Add(new(DiagnosticSeverity.Warning, DiagnosticMessages.TemplateDirectiveIgnored(template.Directive.Location)));
            }

            HashSet<string> names = new HashSet<string>();
            foreach (var param in meta.OfType<Parameter>())
            {
                if (string.IsNullOrEmpty(param.Name) || string.IsNullOrEmpty(param.Type) || param.Name.Trim() != param.Name || param.Type.Trim() != param.Type)
                {
                    Diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.IncorrectParameter(param.Directive.Location)));
                }
                else if (names.Contains(param.Name))
                {
                    Diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.ParameterAlreadyInUse(param.Directive.Location)));
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
                    Diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.IncorrectParameter(line.Directive.Location)));
                }
            }
        }
        return !Diagnostics.ContainsErrors;
    }

    internal async Task GenerateOutput(DirectoryInfo inputDir, DirectoryInfo outputDir, Func<string, TextWriter> generationAction)
    {
        await Task.WhenAll(Inputs.Select(input => GenerateOutputForInput(inputDir, outputDir, input, generationAction)));
    }

    private async Task GenerateOutputForInput(DirectoryInfo inputDir, DirectoryInfo outputDir, FileInfo input, Func<string, TextWriter> generationAction)
    {
        string relDir = Path.Combine(Path.GetRelativePath(Path.GetDirectoryName(input.FullName), inputDir.FullName), input.Name);
        string outFilename = Path.Combine(outputDir.FullName, Path.ChangeExtension(relDir, ".cs"));
        Directory.CreateDirectory(Path.GetDirectoryName(outputDir.FullName));

        await Task.Yield();

        var directives = Directives[input];
        var meta = Meta[input];
        var templateDirective = meta.OfType<Template>().Last();

        using TextWriter wr = generationAction(outFilename);

        TemplateFile tf = new TemplateFile()
        {
            Context = new BaseTemplateContext() { Writer = wr, FormatProvider = System.Globalization.CultureInfo.InvariantCulture },
            Namespace = templateDirective.Namespace,
            Modifier = templateDirective.Visibility,
            ClassName = Path.GetFileNameWithoutExtension(input.Name),
            Meta = meta,
        };

        tf.TransformText(directives);
    }
}
