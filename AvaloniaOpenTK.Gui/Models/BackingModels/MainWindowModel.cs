using Microsoft.Extensions.Logging;

namespace AvaloniaOpenTK.Gui.Models.BackingModels;

public class MainWindowModel
{
    private readonly ILogger<MainWindowModel> m_logger;

    public MainWindowModel(ILogger<MainWindowModel> p_logger)
    {
        m_logger = p_logger;

        m_logger.LogDebug("Creating MainWindowModel");
    }
}