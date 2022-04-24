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
        return await Main(args, null);
    }

    static async Task<int> Main(string[] args, IConsole console)
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
                "recurse input directory directory")
        {
            IsRequired = true
        };
        recursiveOption.AddAlias("-r");

        var rootCommand = new RootCommand
        {
            inputOption,
            outputOption,
            recursiveOption,
        };
        rootCommand.Description = "Quick Template Generator" + Environment.NewLine + "For details visit https://github.com/nicomichaelis/QuickTemplates/";

        rootCommand.SetHandler(async (DirectoryInfo input, DirectoryInfo output, bool recurse, InvocationContext ctx) =>
            {
                Program handler = new(ctx.Console);
                ctx.ExitCode = await handler.Run(input, output, recurse, ctx.GetCancellationToken());
            }, inputOption, outputOption);

        return await rootCommand.InvokeAsync(args, console);
    }
}
