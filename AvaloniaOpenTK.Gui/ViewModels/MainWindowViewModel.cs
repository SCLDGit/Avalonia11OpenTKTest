using AvaloniaOpenTK.Gui.Models.BackingModels;
using Microsoft.Extensions.Logging;

namespace AvaloniaOpenTK.Gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> m_logger;

    public MainWindowViewModel(ILogger<MainWindowViewModel> p_logger,
                               MainWindowModel              p_model)
    {
        m_logger = p_logger;

        m_logger.LogDebug("Creating MainWindowViewModel");

        Model = p_model;
    }
    
    public string Greeting => "Hello World!";

    private MainWindowModel Model { get; }
}