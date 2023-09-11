using Avalonia.OpenGL;
using OpenTK;

namespace AvaloniaOpenTK.Gui.Models.DataStructures.OpenGl;

public class AvaloniaContext : IBindingsContext
{
    private readonly GlInterface m_glInterface;
    
    public AvaloniaContext(GlInterface p_glInterface)
    {
        m_glInterface = p_glInterface;
    }

    public nint GetProcAddress(string p_procName) => m_glInterface.GetProcAddress(p_procName);
}