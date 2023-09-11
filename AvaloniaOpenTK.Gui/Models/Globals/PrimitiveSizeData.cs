using System.Runtime.InteropServices;
using AvaloniaOpenTK.Gui.Models.DataStructures.Primitives;
using OpenTK.Mathematics;

namespace AvaloniaOpenTK.Gui.Models.Globals;

public static class PrimitiveSizeData
{
    public static readonly  int VertexStride  = Marshal.SizeOf<Vertex3D>();
    public static readonly  int Vector3Stride = Marshal.SizeOf<Vector3>();
    private static readonly int Vector2Stride = Marshal.SizeOf<Vector2>();

    // Offsets to vertex data fields.
    // Vertex3D data layout:
    // 1.) World Position: Vector3
    // 2.) Vertex Color: Color4
    // 3.) Vertex Texture Coordinate: Vector2 - Comment by Matt Heimlich on 08/17/2023@22:41:07

    public const           int WorldPositionOffset     = 0;
    public static readonly int ColorOffset             = Vector3Stride;
    public static readonly int TextureCoordinateOffset = Vector2Stride + ColorOffset;
}