using System;
using AvaloniaOpenTK.Gui.Models.Globals;
using OpenTK.Graphics.OpenGL4;
using VertexAttribPointerType = OpenTK.Graphics.OpenGL4.VertexAttribPointerType;

namespace AvaloniaOpenTK.Gui.Models.DataStructures.OpenGl;

public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
    where TVertexType : unmanaged
    where TIndexType : unmanaged
{
    private readonly int m_handle;

    public VertexArrayObject(BufferObject<TVertexType> p_vbo, BufferObject<TIndexType> p_ebo)
    {
        m_handle = GL.GenVertexArray();
        Bind();
        p_vbo.Bind();
        p_ebo.Bind();
    }

    public void VertexAttributePointer(uint p_index, int p_count, VertexAttribPointerType p_type, int p_offSet)
    {
        GL.VertexAttribPointer(p_index, p_count, p_type, false, PrimitiveSizeData.VertexStride, p_offSet);
        GL.EnableVertexAttribArray(p_index);
    }

    public void Bind()
    {
        GL.BindVertexArray(m_handle);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(m_handle);
    }
}