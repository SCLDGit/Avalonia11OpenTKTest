using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaOpenTK.Gui.Models.BackingModels;
using AvaloniaOpenTK.Gui.Models.DataStructures.Logging;
using AvaloniaOpenTK.Gui.Models.Globals;
using AvaloniaOpenTK.Gui.Models.Utilities;
using AvaloniaOpenTK.Gui.ViewModels;
using AvaloniaOpenTK.Gui.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AvaloniaOpenTK.Gui
{
    public class AvaloniaOpenTKGuiApp : Application
    {
        private readonly IHost m_appHost;

        public AvaloniaOpenTKGuiApp()
        {
            m_appHost = Host.CreateDefaultBuilder()
                            .ConfigureServices(ConfigureServices)
                            .ConfigureLogging(ConfigureLogging)
                            .Build();
        }

        private static void ConfigureLogging(HostBuilderContext p_context, ILoggingBuilder p_builder)
        {
            var configuredLogLevel =
                LogLevelUtilities.GetLogLevel(p_context.Configuration
                                                  ["Logging:LogLevel:Default"]);

            p_builder.ClearProviders();

            if (configuredLogLevel < LogLevel.Information)
            {
                // Add logging to debug console. - Comment by Matt Heimlich on 07/10/2023@11:25:47
                p_builder.AddDebug();
            }

            // Add logging to file defined in appsettings.json - Comment by Matt Heimlich on 07/10/2023@11:27:39
            p_builder.AddFile(CommonFiles.LogFilePath,
                              configuredLogLevel,
                              retainedFileCountLimit: 31,
                              fileSizeLimitBytes: 1024 * 1024 * 10);

            // Prepare Serilog logger to log to collection sink. - Comment by Matt Heimlich on 07/10/2023@11:28:02
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel
                        .Is(LogLevelUtilities
                               .GetSerilogLogLevel(configuredLogLevel))
                        .WriteTo.Sink(new CollectionSink())
                        .CreateLogger();

            // Add logging to collection sink. - Comment by Matt Heimlich on 07/10/2023@11:28:22
            p_builder.AddSerilog(Log.Logger);
        }

        private static void ConfigureServices(IServiceCollection p_serviceCollection)
        {
            p_serviceCollection.AddSingleton<MainWindowModel>();
            p_serviceCollection.AddSingleton<MainWindowViewModel>();
            p_serviceCollection.AddSingleton<MainWindowView>();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            await m_appHost.StartAsync();
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownRequested += OnShutdownRequested;

                InitializeApplicationAsync();

                desktop.MainWindow = m_appHost.Services.GetRequiredService<MainWindowView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        private static void InitializeApplicationAsync()
        {
            CommonDirectories.CreateRequiredDirectories();
        }
        
        private async void OnShutdownRequested(object? p_sender, ShutdownRequestedEventArgs p_e)
        {
            CommonDirectories.CleanUp();
            
            await m_appHost.StopAsync();
        }
    }
}