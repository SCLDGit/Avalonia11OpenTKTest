using Serilog.Events;

namespace AvaloniaOpenTK.Gui.Models.DataStructures.Logging;

public class ConsoleLogMessage
{
    public LogEventLevel LogLevel { get; set; }
    public string? Text { get; set; }
}