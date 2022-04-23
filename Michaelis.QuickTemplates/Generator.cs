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
    private DiagnosticsCollection Diagnostics = new();

    public Generator(List<FileInfo> inputs)
    {
        Inputs = inputs;
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

        WriteDiagnostics();
        return !Diagnostics.ContainsErrors;
    }

    private void WriteDiagnostics()
    {
        foreach (var diagnostic in Diagnostics.Diagnostics.OrderBy(z => z.Message.Location.SourceName, NaturalSortComparer.Default).ThenBy(z => z.Message.Location.Line).ThenBy(z => z.Message.Location.Col))
        {
            Console.Error.WriteLine($"{diagnostic.Message.Location.SourceName}({diagnostic.Message.Location.Line},{diagnostic.Message.Location.Col}): {diagnostic.Severity}: {diagnostic.Message.Text}");
        }
        Diagnostics.Clear();
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

        WriteDiagnostics();
        return !Diagnostics.ContainsErrors;
    }

    private (FileInfo file, List<MetaData> metadata) ReadFileMeta(FileInfo inFile)
    {
        var meta = new List<MetaData>();
        MetaReader rdr = new MetaReader();
        foreach (var dir in Directives[inFile])
        {
            if (dir.Mode == DirectiveMode.Meta)
            {
                var res = rdr.Decode(dir, Diagnostics);
                if (res.success)
                {
                    meta.Add((MetaData)res.result);
                    ((MetaData)res.result).Directive = dir;
                }
            }
        }
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
        }
        return !Diagnostics.ContainsErrors;
    }

    internal async Task GenerateOutput(DirectoryInfo inputDir, DirectoryInfo outputDir)
    {
        try
        {
            await Task.WhenAll(Inputs.Select(input => GenerateOutputForInput(inputDir, outputDir, input)));
        }
        finally
        {
            WriteDiagnostics();
        }
    }

    private async Task GenerateOutputForInput(DirectoryInfo inputDir, DirectoryInfo outputDir, FileInfo input)
    {
        string relDir = Path.Combine(Path.GetRelativePath(Path.GetDirectoryName(input.FullName), inputDir.FullName), input.Name);
        string outFilename = Path.Combine(outputDir.FullName, Path.ChangeExtension(relDir, ".cs"));
        Directory.CreateDirectory(Path.GetDirectoryName(outputDir.FullName));

        await Task.Yield();

        var directives = Directives[input];
        var meta = Meta[input];
        var templateDirective = meta.OfType<Template>().Last();
        using (ChangeStream cs = new ChangeStream(File.Open(outFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), false))
        {
            using (TextWriter wr = new StreamWriter(cs))
            {
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
            if (cs.Updated)
            {
                Diagnostics.Add(new(DiagnosticSeverity.Info, DiagnosticMessages.FileUpdated(outFilename)));
            }
        }
    }
}
