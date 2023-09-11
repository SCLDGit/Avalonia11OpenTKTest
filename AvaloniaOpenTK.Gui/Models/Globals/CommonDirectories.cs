using System;
using System.IO;

namespace AvaloniaOpenTK.Gui.Models.Globals;

public static class CommonDirectories
{
    #if DEBUG
    public static string RootDataPath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "Avalonia OpenTK Test", "GUI", "Debug");
    #else
    public static string RootDataPath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "Avalonia OpenTK Test", "GUI");
    #endif
    
    public static string LogsDataPath => Path.Combine(RootDataPath, "Logs");

    public static string TempDocumentDataPath => Path.Combine(Path.GetTempPath(), "Avalonia OpenTK Test");

    public static void CreateRequiredDirectories()
    {
        // Logs data path is automatically created by ILogger. - Comment by Matt Heimlich on 07/11/2023@12:54:05
        Directory.CreateDirectory(RootDataPath);
        Directory.CreateDirectory(TempDocumentDataPath);
    }

    public static void CleanUp()
    {
        if ( Directory.Exists(TempDocumentDataPath) )
        {
            Directory.Delete(TempDocumentDataPath, true);
        }
    }
}