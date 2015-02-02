using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Windows;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using System.Runtime.InteropServices;

namespace KirosEngine.Shader
{
    /// <summary>
    /// Structure deffinition for a buffer to pass matrix data to a vertex shader.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct MatrixBufferType
    {
        public Matrix world;
        public Matrix view;
        public Matrix projection;
    };

    /// <summary>
    /// Structure deffinition for a vertex with position and a texture.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct TexturedVertex
    {
        public Vector3 position;
        public Vector2 texture;
    };

    /// <summary>
    /// Structure deffinition for a light to pass to a shader
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct LightBufferType
    {
        public Color4 ambientColor;
        public Color4 diffuseColor;
        public Vector3 lightDirection;
        public float specularPower;
        public Color4 specularColor;
    }

    /// <summary>
    /// Structure deffinition for a camera to pass to a shader
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct CameraBufferType
    {
        public Vector3 cameraPosition;
        public float padding;
    }

    /// <summary>
    /// Structure deffinition for a pixel buffer
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct PixelBufferType
    {
        public Vector4 pixelColor;
    }
}