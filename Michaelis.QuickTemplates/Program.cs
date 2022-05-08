using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

class Program
{
    IConsole Console { get; }

    public Program(IConsole console)
    {
        Console = console;
    }

    public async Task<int> Run(DirectoryInfo input, DirectoryInfo output, bool recurse, bool prototype, CancellationToken cancel)
    {
        var searchOptions = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var inputs = input.EnumerateFiles("*.tt", searchOptions).OrderBy(z => z.Name, NaturalSortComparer.Default).ToList();

        DiagnosticsCollection diagnostics = new();
        try
        {
            if (prototype)
            {
                var writer = new FileWriter(output);
                var inputReader = new FileInputReader(input, inputs);
                TemplateGenerator generator = new(inputReader, writer, diagnostics);
                return await generator.Run(cancel);
            }

            Generator gen = new Generator(inputs, diagnostics);
            if (!await gen.ReadDirectives(cancel))
            {
                return -1;
            }

            if (!await gen.ReadMeta())
            {
                return -2;
            }

            if (!gen.VerifyMeta())
            {
                return -3;
            }

            await gen.GenerateOutput(input, output, CreateFileWriter);
        }
        finally
        {
            WriteDiagnostics(diagnostics);
        }
        return 0;
    }

    static TextWriter CreateFileWriter(string filename)
    {
        var cs = new ChangeStream(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), false);
        return new StreamWriter(cs);
    }

    void WriteDiagnostics(DiagnosticsCollection diagnostics)
    {
        foreach (var diagnostic in diagnostics.Diagnostics.OrderBy(z => z.Message.Location.SourceName, NaturalSortComparer.Default).ThenBy(z => z.Message.Location.Line).ThenBy(z => z.Message.Location.Col))
        {
            if (diagnostic.Message.Location != null)
            {
                Console.Error.WriteLine($"{diagnostic.Message.Location.SourceName}({diagnostic.Message.Location.Line},{diagnostic.Message.Location.Col}): {diagnostic.Severity}: {diagnostic.Message.Text}");
            }
            else
            {
                Console.Error.WriteLine($"{diagnostic.Severity}: {diagnostic.Message.Text}");
            }
        }
    }

}