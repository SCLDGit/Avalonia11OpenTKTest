using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaOpenTK.Gui.ViewModels;
using Microsoft.Extensions.Logging;

namespace AvaloniaOpenTK.Gui.Views;

public partial class MainWindowView : Window
{
    private readonly ILogger<MainWindowView> m_logger;

    #pragma warning disable CS8618
    public MainWindowView()
    {
    }
    #pragma warning restore CS8618

    public MainWindowView(ILogger<MainWindowView> p_logger,
                          MainWindowViewModel     p_viewModel)
    {
        m_logger = p_logger;

        m_logger.LogDebug("Creating MainWindowView");

        InitializeComponent();

        DataContext = p_viewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}