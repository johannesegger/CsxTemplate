using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell.Interop;

namespace CsxTemplate
{
    public static class VsGeneratorProgressExtensions
    {
        public static void YieldDiagnostics(this IVsGeneratorProgress progress, IEnumerable<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                progress.YieldDiagnostic(diagnostic);
            }
        }

        public static void YieldDiagnostic(this IVsGeneratorProgress progress, Diagnostic diagnostic)
        {
            var position = diagnostic.Location.GetLineSpan().StartLinePosition;
            if (diagnostic.Severity == DiagnosticSeverity.Error)
            {
                progress.YieldError(diagnostic.GetMessage(), position.Line, position.Character);
            }
            else
            {
                progress.YieldWarning(diagnostic.GetMessage(), position.Line, position.Character);
            }
        }

        public static void YieldWarning(this IVsGeneratorProgress progress, string message, int line, int column)
        {
            progress.GeneratorError(1, 0u, message, (uint)line, (uint)column);
        }

        public static void YieldError(this IVsGeneratorProgress progress, string message, int line, int column)
        {
            progress.GeneratorError(0, 0u, message, (uint) line, (uint) column);
        }
    }
}
