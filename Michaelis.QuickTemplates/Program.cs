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

        Generator gen = new Generator(inputs);
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

        await gen.GenerateOutput(input, output);
        return 0;
    }
}
