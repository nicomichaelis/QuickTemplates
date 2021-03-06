using System;
using Michaelis.QuickTemplates.Text;

namespace Michaelis.QuickTemplates;

public interface DiagnosticMessage
{
    TextLocation Location { get; }
    string Text { get; }
}

public static class DiagnosticMessages
{
    public static DiagnosticMessage Exception(Exception e, TextLocation location) => new DiagnosticMessageInstance(location, $"Unexpected Exception: ({e.GetType().FullName}) {e.Message}");
    public static DiagnosticMessage MalformedDirective(TextLocation location) => new DiagnosticMessageInstance(location, $"malformed directive");
    public static DiagnosticMessage DirectiveUnknown(TextLocation location, string dirName) => new DiagnosticMessageInstance(location, $"unknown directive '{dirName}'");
    public static DiagnosticMessage PropertyUnknown(TextLocation location, string propName) => new DiagnosticMessageInstance(location, $"unknown property '{propName}'");
    public static DiagnosticMessage MalformedValue(TextLocation location, string value) => new DiagnosticMessageInstance(location, $"malformed value '{value}'");
    public static DiagnosticMessage FileUpdated(string filename) => new DiagnosticMessageInstance(new TextLocation(filename, 1, 1), $"file updated");
    public static DiagnosticMessage RequiredPropertyMissing(TextLocation location, string property) => new DiagnosticMessageInstance(location, $"missing required property '{property}'");
    public static DiagnosticMessage TemplateDirectiveMissing(string filename) => new DiagnosticMessageInstance(new TextLocation(filename, 1, 1), $"template directive missing");
    public static DiagnosticMessage MultipleTemplateDirectives(TextLocation location) => new DiagnosticMessageInstance(location, $"multiple template directives");
    public static DiagnosticMessage IncorrectParameter(TextLocation location) => new DiagnosticMessageInstance(location, $"parameter not correct");
    public static DiagnosticMessage ParameterAlreadyInUse(TextLocation location) => new DiagnosticMessageInstance(location, $"parameter already in use");
    public static DiagnosticMessage TemplateNameNotUnique(TextLocation location) => new DiagnosticMessageInstance(location, $"template name not unique");
    public static DiagnosticMessage BaseNotFound(TextLocation location, string inheritFrom) => new DiagnosticMessageInstance(location, $"the template '{inheritFrom}' was not found");
    public static DiagnosticMessage InfiniteRecursion(TextLocation location) => new DiagnosticMessageInstance(location, $"the inheritance induces infinite recursion");

    record DiagnosticMessageInstance(TextLocation Location, string Text) : DiagnosticMessage
    {
    }

}

