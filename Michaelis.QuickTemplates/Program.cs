using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

class Program
{
    static int returnCode = int.MinValue;

    static async Task<int> Main(string[] args)
    {
        var inputOption = new Option<DirectoryInfo>(
                "--input",
                getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory),
                "input directory")
        {
            IsRequired = true
        }.ExistingOnly();

        var outputOption = new Option<DirectoryInfo>(
                "--output",
                getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory),
                "output directory")
        {
            IsRequired = true
        };


        var rootCommand = new RootCommand
        {
            inputOption,
            outputOption,
        };

        rootCommand.Description = "Mini Template Generator";

        rootCommand.SetHandler(async (DirectoryInfo input, DirectoryInfo output, CancellationToken cancel) =>
            {
                CreateDirectory(output);
                returnCode = await RunTemplateGeneratorOnDirectory(input, output, cancel);
            }, inputOption, outputOption);

        await rootCommand.InvokeAsync(args);
        return returnCode;
    }

    private static void CreateDirectory(DirectoryInfo output)
    {
        if (!output.Exists)
        {
            Console.WriteLine($"Creating directory '{output.FullName}'");
            try
            {
                output.Create();
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"Output directory could not be created: {e.Message}");
            }
        }
    }

    private static async Task<int> RunTemplateGeneratorOnDirectory(DirectoryInfo input, DirectoryInfo output, CancellationToken cancel)
    {
        var inputs = input.EnumerateFiles("*.tt", SearchOption.TopDirectoryOnly).OrderBy(z => z.Name, NaturalSortComparer.Default).ToList();

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

    private static TextWriter CreateFileWriter(string filename) {
        var cs = new ChangeStream(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), false);
        return new StreamWriter(cs);
    }

    private static void WriteDiagnostics(DiagnosticsCollection diagnostics)
    {
        foreach (var diagnostic in diagnostics.Diagnostics.OrderBy(z => z.Message.Location.SourceName, NaturalSortComparer.Default).ThenBy(z => z.Message.Location.Line).ThenBy(z => z.Message.Location.Col))
        {
            Console.Error.WriteLine($"{diagnostic.Message.Location.SourceName}({diagnostic.Message.Location.Line},{diagnostic.Message.Location.Col}): {diagnostic.Severity}: {diagnostic.Message.Text}");
        }
    }
}
