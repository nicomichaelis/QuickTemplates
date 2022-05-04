using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

class EntryPoint
{
    static async Task<int> Main(string[] args)
    {
        return await Run(args, null);
    }

    static async Task<int> Run(string[] args, IConsole console)
    {
        var inputOption = new Option<DirectoryInfo>(
                "--input",
                getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory),
                "input directory")
        {
            IsRequired = true
        }.ExistingOnly();
        inputOption.AddAlias("-i");

        var outputOption = new Option<DirectoryInfo>(
                "--output",
                getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory),
                "output directory")
        {
            IsRequired = true
        };
        outputOption.AddAlias("-o");

        var recursiveOption = new Option<bool>(
                "--recurse",
                "recurse input directory directory");
        recursiveOption.AddAlias("-r");

        var prototypeOption = new Option<bool>(
                "--prototype"
                )
        { IsHidden = true };
        recursiveOption.AddAlias("-r");

        var rootCommand = new RootCommand
        {
            inputOption,
            outputOption,
            recursiveOption,
            prototypeOption,
        };
        rootCommand.Description = "Quick Template Generator"
            + Environment.NewLine + "Tool for generating runtime text templates"
            + Environment.NewLine + "For details visit https://github.com/nicomichaelis/QuickTemplates/";

        rootCommand.SetHandler(async (DirectoryInfo input, DirectoryInfo output, bool recurse, bool prototype, InvocationContext ctx) =>
            {
                Program handler = new(ctx.Console);
                ctx.ExitCode = await handler.Run(input, output, recurse, prototype, ctx.GetCancellationToken());
            }, inputOption, outputOption, recursiveOption, prototypeOption);

        return await rootCommand.InvokeAsync(args, console);
    }
}
