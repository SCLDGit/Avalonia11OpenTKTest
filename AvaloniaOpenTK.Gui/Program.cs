using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace AvaloniaOpenTK.Gui
{
    internal static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] p_args) => BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(p_args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<AvaloniaOpenTKGuiApp>()
                         .UsePlatformDetect()
                         .With(new Win32PlatformOptions
                               {
                                   RenderingMode = new [] { Win32RenderingMode.Wgl }
                               })
                         .LogToTrace()
                         .UseReactiveUI();
    }
}