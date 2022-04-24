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
    DiagnosticsCollection Diagnostics { get; } = new();

    public Program(IConsole console)
    {
        Console = console;
    }

    public async Task<int> Run(DirectoryInfo input, DirectoryInfo output, bool recurse, CancellationToken cancel)
    {
        var searchOptions = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var inputs = input.EnumerateFiles("*.tt", searchOptions).OrderBy(z => z.Name, NaturalSortComparer.Default).ToList();

        DiagnosticsCollection diagnostics = new();
        Generator gen = new Generator(inputs, diagnostics);
        try
        {
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

    private bool CreateDirectory(DirectoryInfo output)
    {
        if (!output.Exists)
        {
            Console.Out.WriteLine($"Creating directory '{output.FullName}'");
            try
            {
                output.Create();
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"Output directory could not be created: {e.Message}");
                return false;
            }
        }
        return true;
    }

    private static TextWriter CreateFileWriter(string filename)
    {
        var cs = new ChangeStream(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), false);
        return new StreamWriter(cs);
    }

    private void WriteDiagnostics(DiagnosticsCollection diagnostics)
    {
        foreach (var diagnostic in diagnostics.Diagnostics.OrderBy(z => z.Message.Location.SourceName, NaturalSortComparer.Default).ThenBy(z => z.Message.Location.Line).ThenBy(z => z.Message.Location.Col))
        {
            Console.Error.WriteLine($"{diagnostic.Message.Location.SourceName}({diagnostic.Message.Location.Line},{diagnostic.Message.Location.Col}): {diagnostic.Severity}: {diagnostic.Message.Text}");
        }
    }

}