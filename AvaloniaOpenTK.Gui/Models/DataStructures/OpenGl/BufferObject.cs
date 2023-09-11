using System;
using System.Runtime.InteropServices;
using AvaloniaOpenTK.Gui.Models.DataStructures.Primitives;
using AvaloniaOpenTK.Gui.Models.Globals;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AvaloniaOpenTK.Gui.Models.DataStructures.OpenGl;

public class BufferObject<TDataType> : IDisposable
    where TDataType : unmanaged
{
    private int          m_handle;
    private BufferTarget m_bufferType;

    public BufferObject(Span<TDataType> p_data, BufferTarget p_bufferType)
    {
        m_bufferType = p_bufferType;

        m_handle = GL.GenBuffer();

        Bind();

        var stride = GetStride<TDataType>();

        GL.BufferData(p_bufferType, stride * p_data.Length, p_data.ToArray(), BufferUsageHint.StaticDraw);
    }

    private static int GetStride<T>() where T : unmanaged
    {
        return typeof(T) switch
               {
                   { } vertexType when vertexType   == typeof(Vertex3D) => PrimitiveSizeData.VertexStride,
                   { } vector3Type when vector3Type == typeof(Vector3)  => PrimitiveSizeData.Vector3Stride,
                   { } uintType when uintType       == typeof(uint)     => sizeof(uint),
                   { IsPrimitive: true }                                => Marshal.SizeOf<T>(),
                   _                                                    => throw new ArgumentOutOfRangeException()
               };
    }

    public void Bind()
    {
        GL.BindBuffer(m_bufferType, m_handle);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(m_handle);
    }
}