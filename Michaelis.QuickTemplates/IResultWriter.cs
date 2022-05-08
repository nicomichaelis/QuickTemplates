namespace Michaelis.QuickTemplates;

internal interface IResultWriter
{
}

public record OutputData(string SourceName, object Result); // TODO -> object to base template?
