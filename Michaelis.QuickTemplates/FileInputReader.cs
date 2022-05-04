using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

internal class FileInputReader : IInputReader
{
    private List<FileInfo> InputFiles;

    public FileInputReader(List<FileInfo> inputs)
    {
        InputFiles = inputs ?? throw new ArgumentNullException(nameof(inputs));
    }

    public IEnumerable<Task<InputData>> GetInputs(CancellationToken cancel)
    {
        return InputFiles.Select(z => ReadInput(z, cancel));
    }

    private async Task<InputData> ReadInput(FileInfo fileinfo, CancellationToken cancel)
    {
        await Task.Yield();
        return new InputData(fileinfo.FullName, await File.ReadAllTextAsync(fileinfo.FullName, cancel));

    }
}
