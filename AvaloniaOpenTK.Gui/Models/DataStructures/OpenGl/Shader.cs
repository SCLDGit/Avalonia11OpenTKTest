using System;
using System.IO;
using Avalonia.Platform;
using AvaloniaOpenTK.Gui.Models.Enumerations;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace AvaloniaOpenTK.Gui.Models.DataStructures.OpenGl;

public class Shader : IDisposable
{
    private readonly int m_handle;

    public Shader(ShaderReadMode p_readMode, string p_vertexShaderPath, string p_fragmentShaderPath)
    {
        var vertex =
            p_readMode switch
            {
                ShaderReadMode.ASSET => LoadShaderFromAsset(ShaderType.VertexShader, p_vertexShaderPath),
                ShaderReadMode.FILE  => LoadShaderFromFile(ShaderType.VertexShader, p_vertexShaderPath),
                _                    => throw new ArgumentOutOfRangeException(nameof(p_readMode), p_readMode, null)
            };

        var fragment =
            p_readMode switch
            {
                ShaderReadMode.ASSET => LoadShaderFromAsset(ShaderType.FragmentShader, p_fragmentShaderPath),
                ShaderReadMode.FILE  => LoadShaderFromFile(ShaderType.FragmentShader, p_fragmentShaderPath),
                _                    => throw new ArgumentOutOfRangeException(nameof(p_readMode), p_readMode, null)
            };

        m_handle = GL.CreateProgram();

        GL.AttachShader(m_handle, vertex);
        GL.AttachShader(m_handle, fragment);
        GL.LinkProgram(m_handle);

        GL.GetProgram(m_handle, GetProgramParameterName.LinkStatus, out var status);

        if (status == 0)
        {
            throw new Exception($"Program failed to link with error: {GL.GetProgramInfoLog(m_handle)}");
        }

        GL.DetachShader(m_handle, vertex);
        GL.DetachShader(m_handle, fragment);
        GL.DeleteShader(vertex);
        GL.DeleteShader(fragment);
    }

    public void Use()
    {
        GL.UseProgram(m_handle);
    }

    public void SetUniform(string p_name, int p_value)
    {
        var location = GL.GetUniformLocation(m_handle, p_name);
        if (location == -1)
        {
            throw new Exception($"{p_name} uniform not found on shader.");
        }

        GL.Uniform1(location, p_value);
    }

    public void SetUniform(string p_name, float p_value)
    {
        var location = GL.GetUniformLocation(m_handle, p_name);
        if (location == -1)
        {
            throw new Exception($"{p_name} uniform not found on shader.");
        }

        GL.Uniform1(location, p_value);
    }

    public void SetUniform2(string p_name, Vector2 p_value)
    {
        var location = GL.GetUniformLocation(m_handle, p_name);
        if (location == -1)
        {
            throw new Exception($"{p_name} uniform not found on shader.");
        }

        GL.Uniform2(location, p_value);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GL.DeleteProgram(m_handle);
    }

    private int LoadShaderFromAsset(ShaderType p_type, string p_shaderName)
    {
        var assetStream = AssetLoader.Open(new Uri($"avares://Avalonia OpenTK Test/Assets/{p_shaderName}"));
        var src         = new StreamReader(assetStream).ReadToEnd();
        var handle      = GL.CreateShader(p_type);

        GL.ShaderSource(handle, src);
        GL.CompileShader(handle);

        var infoLog = GL.GetShaderInfoLog(handle);

        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {p_type}, failed with error {infoLog}");
        }

        return handle;
    }

    private int LoadShaderFromFile(ShaderType p_type, string p_path)
    {
        var src    = File.ReadAllText(p_path);
        var handle = GL.CreateShader(p_type);
        GL.ShaderSource(handle, src);
        GL.CompileShader(handle);

        var infoLog = GL.GetShaderInfoLog(handle);

        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {p_type}, failed with error {infoLog}");
        }

        return handle;
    }
}