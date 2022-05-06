using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

internal class FileInputReader : IInputReader
{
    readonly List<FileInfo> _inputFiles;

    public FileInputReader(List<FileInfo> inputs)
    {
        _inputFiles = inputs ?? throw new ArgumentNullException(nameof(inputs));
    }

    public IEnumerable<Task<InputData>> GetInputs(CancellationToken cancel)
    {
        return _inputFiles.Select(z => ReadInput(z, cancel));
    }

    async Task<InputData> ReadInput(FileInfo fileinfo, CancellationToken cancel)
    {
        await Task.Yield();
        return new InputData(fileinfo.FullName, await File.ReadAllTextAsync(fileinfo.FullName, cancel));

    }
}
