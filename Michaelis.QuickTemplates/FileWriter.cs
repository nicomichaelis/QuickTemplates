using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

internal class FileWriter : IResultWriter
{
    readonly DirectoryInfo _output;
    DiagnosticsCollection Diagnostics { get; }

    public FileWriter(DirectoryInfo output, DiagnosticsCollection diagnostics)
    {
        _output = output;
        Diagnostics = diagnostics;
    }

    public async Task Write(List<OutputData> outputData)
    {
        await Task.WhenAll(outputData.Select(WriteData).ToList());
    }

    async Task WriteData(OutputData data)
    {
        await Task.Yield();
        string filename = Path.Combine(_output.FullName, data.SourceName);
        lock (_output)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
        }
        using var cs = new ChangeStream(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), false);
        using var writer = new StreamWriter(cs);
        data.WriteAction(writer);
        if (cs.Updated)
        {
            Diagnostics.Add(new Diagnostic(DiagnosticSeverity.Info, DiagnosticMessages.FileUpdated(filename)));
        }
    }
}
