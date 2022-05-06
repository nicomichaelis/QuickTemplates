using System.Collections.Generic;
using System.Linq;

namespace Michaelis.QuickTemplates;

public class DiagnosticsCollection
{
    object _locker = new();
    List<Diagnostic> _diagnostics = new();

    public void Add(Diagnostic diagnostic)
    {
        lock (_locker)
        {
            _diagnostics.Add(diagnostic);
            if (diagnostic.Severity == DiagnosticSeverity.Error)
            {
                ContainsErrors = true;
            }
        }
    }

    public bool ContainsErrors { get; private set; }

    public IEnumerable<Diagnostic> Diagnostics
    {
        get
        {
            lock (_locker) return _diagnostics.ToList();
        }
    }

    public void Clear()
    {
        lock (_locker)
        {
            _diagnostics.Clear();
        }
    }
}
