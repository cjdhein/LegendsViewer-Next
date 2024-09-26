using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace LegendsViewer.Backend.Logging;
public class SimpleLogFormatter : ConsoleFormatter
{
    public SimpleLogFormatter() : base(nameof(SimpleLogFormatter))
    {
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        // Write only the log message, no category or log level
        textWriter.WriteLine(logEntry.State?.ToString());
    }
}
