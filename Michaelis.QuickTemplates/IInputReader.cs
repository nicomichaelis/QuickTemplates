using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

public interface IInputReader
{
    IEnumerable<Task<InputData>> GetInputs(CancellationToken cancel);
}

public record InputData(string SourceName, string SourceText);
