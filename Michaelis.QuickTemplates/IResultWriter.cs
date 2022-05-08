using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Michaelis.QuickTemplates;

internal interface IResultWriter
{
    Task Write(List<OutputData> outputData);
}

public record OutputData(string SourceName, Action<TextWriter> WriteAction);
