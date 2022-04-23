namespace Michaelis.QuickTemplates;

public enum DiagnosticSeverity { Info, Warning, Error }

public record Diagnostic(DiagnosticSeverity Severity, DiagnosticMessage Message)
{
}
