using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

internal class FileInputReader : IInputReader
{
    readonly DirectoryInfo _inputFolder;
    readonly List<FileInfo> _inputFiles;

    public FileInputReader(DirectoryInfo inputFolder, List<FileInfo> inputs)
    {
        _inputFolder = inputFolder ?? throw new ArgumentNullException(nameof(inputFolder));
        _inputFiles = inputs ?? throw new ArgumentNullException(nameof(inputs));
    }

    public IEnumerable<Task<InputData>> GetInputs(CancellationToken cancel)
    {
        return _inputFiles.Select(z => ReadInput(z, cancel));
    }

    async Task<InputData> ReadInput(FileInfo fileinfo, CancellationToken cancel)
    {
        await Task.Yield();
        return new InputData(GetRelName(fileinfo), fileinfo.FullName, await File.ReadAllTextAsync(fileinfo.FullName, cancel));
    }

    string GetRelName(FileInfo input)
    {
        return Path.Combine(Path.GetRelativePath(Path.GetDirectoryName(input.FullName), _inputFolder.FullName), input.Name);
    }
}
