using System.Diagnostics;
using System.Drawing;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using AvaloniaOpenTK.Gui.Models.DataStructures.OpenGl;
using AvaloniaOpenTK.Gui.Models.DataStructures.Primitives;
using AvaloniaOpenTK.Gui.Models.Enumerations;
using AvaloniaOpenTK.Gui.Models.Globals;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AvaloniaOpenTK.Gui.Views;

public class OpenGlView : OpenGlControlBase
{
    private          BufferObject<Vertex3D>?            m_vbo;
    private          BufferObject<uint>?                m_ebo;
    private          VertexArrayObject<Vertex3D, uint>? m_vao;
    private          Shader?                            m_shader;
    private readonly Stopwatch                          m_time;

    public OpenGlView()
    {
        m_time = Stopwatch.StartNew();
    }

    private static readonly Vertex3D[] Vertices =
    {
        new (new Vector3(1.0f, 1.0f, 0.0f), new Color4(0.0f, 0.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)),
        new (new Vector3( 1.0f, -1.0f, 0.0f), new Color4(0.0f, 0.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)),
        new (new Vector3(-1.0f, -1.0f, 0.0f), new Color4( 0.0f, 0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)),
        new (new Vector3(-1.0f, 1.0f, 1.0f), new Color4(0.0f, 0.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f))
    };

    private static readonly uint[] Indices =
    {
        0, 1, 3,
        1, 2, 3
    };

    protected override void OnOpenGlInit(GlInterface p_gl)
    {
        base.OnOpenGlInit(p_gl);
        
        GL.LoadBindings(new AvaloniaContext(p_gl));
        
        //Instantiating our new abstractions
        m_ebo = new BufferObject<uint>(Indices, BufferTarget.ElementArrayBuffer);
        m_vbo = new BufferObject<Vertex3D>(Vertices, BufferTarget.ArrayBuffer);
        m_vao = new VertexArrayObject<Vertex3D, uint>(m_vbo, m_ebo);

        //Telling the VAO object how to lay out the attribute pointers
        m_vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, PrimitiveSizeData.WorldPositionOffset);
        m_vao.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, PrimitiveSizeData.ColorOffset);
        m_vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, PrimitiveSizeData.TextureCoordinateOffset);

        m_shader = new Shader(ShaderReadMode.ASSET, "backgroundShader.vert", "earthboundShader.frag");
    }

    protected override void OnOpenGlDeinit(GlInterface p_gl)
    {
        m_vbo?.Dispose();
        m_ebo?.Dispose();
        m_vao?.Dispose();
        m_shader?.Dispose();
        base.OnOpenGlDeinit(p_gl);
    }

    protected override void OnOpenGlRender(GlInterface p_gl, int p_fb)
    {
        GL.ClearColor(Color.Firebrick);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.DepthTest);
        GL.Viewport(0, 0, (int) Bounds.Width, (int) Bounds.Height);

        m_ebo?.Bind();
        m_vbo?.Bind();
        m_vao?.Bind();
        m_shader?.Use();
        m_shader?.SetUniform("time", (float) m_time.Elapsed.TotalSeconds);
        m_shader?.SetUniform2("resolution", new Vector2((float) Bounds.Width, (float) Bounds.Height));

        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
    }
}