using System.IO;

namespace AvaloniaOpenTK.Gui.Models.Globals;

public static class CommonFiles
{
    public static string LogFilePath => Path.Combine(CommonDirectories.LogsDataPath, "activity.log");
}