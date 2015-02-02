using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using System.Runtime.InteropServices;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Textures
{
    /// <summary>
    /// Constructs a vertex array to use in the drawing of a 2d texture
    /// </summary>
    class Bitmap : IDisposable
    {
        private Device _device;
        private Buffer _vertexBuffer;
        private Buffer _indexBuffer;
        private int _vertexCount;
        private int _indexCount;
        private Texture _texture;
        private int _screenWidth, _screenHeight;
        private int _bitmapWidth, _bitmapHeight;
        private int _previousX, _previousY;

        /// <summary>
        /// Initalize the bitmap
        /// </summary>
        /// <param name="device">DirectX device</param>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="screenHeight">The height of the screen</param>
        /// <param name="fileName">The file containing the texture</param>
        /// <param name="bitmapWidth">The width of the bitmap</param>
        /// <param name="bitmapHeight">The height of the bitmap</param>
        public void Initialize(Device device, int screenWidth, int screenHeight, string fileName, int bitmapWidth, int bitmapHeight)
        {
            _device = device;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _bitmapHeight = bitmapHeight;
            _bitmapWidth = bitmapWidth;

            _previousX = -1;
            _previousY = -1;

            if (!LoadTexture(fileName))
            {
                throw new System.IO.IOException(String.Format("Failed to load the specified bitmap texture from: {0}", fileName));
            }

            //init buffers
            _vertexCount = 6;
            _indexCount = 6;

            BufferDescription indexBufferDisc;

            indexBufferDisc = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = sizeof(uint) * _indexCount,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            //fill the index stream
            DataStream indexStream = new DataStream(sizeof(uint) * _indexCount, true, true);
            for (int i = 0; i < _indexCount; i++)
            {
                indexStream.Write((uint)i);
            }
            indexStream.Position = 0;

            //if there is already a buffer dispose of it
            if (_indexBuffer != null)
            {
                _indexBuffer.Dispose();
            }
            _indexBuffer = new Buffer(_device, indexStream, indexBufferDisc);

            indexStream.Close();
            indexStream.Dispose();
        }

        /// <summary>
        /// Sets the vertex buffer for the given positions
        /// </summary>
        /// <param name="positionX">The starting X position</param>
        /// <param name="positionY">The starting Y position</param>
        private void SetVertexBuffer(int positionX, int positionY)
        {
            float top, right, left, bottom;
            BufferDescription vertBufferDisc;

            //if the position is the same no need to update
            if (positionX == _previousX && positionY == _previousY)
            {
                return;
            }

            vertBufferDisc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(typeof(Vertex2D)) * _vertexCount,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            _previousX = positionX;
            _previousY = positionY;

            left = (float)(_screenWidth / 2 * -1) + (float)positionX;
            right = left + (float)_bitmapWidth;
            top = (float)(_screenHeight / 2) - (float)positionY;
            bottom = top - (float)_bitmapHeight;

            Vertex2D[] vertices = new Vertex2D[_vertexCount];

            //tri 1
            //top left
            vertices[0].position = new Vector3(left, top, 0.0f);
            vertices[0].texture = new Vector2(0.0f, 0.0f);

            //bottom right
            vertices[1].position = new Vector3(right, bottom, 0.0f);
            vertices[1].texture = new Vector2(1.0f, 1.0f);

            //bottom left
            vertices[2].position = new Vector3(left, bottom, 0.0f);
            vertices[2].texture = new Vector2(0.0f, 1.0f);

            //tri 2
            //top left
            vertices[3].position = new Vector3(left, top, 0.0f);
            vertices[3].texture = new Vector2(0.0f, 0.0f);

            //top right
            vertices[4].position = new Vector3(right, top, 0.0f);
            vertices[4].texture = new Vector2(1.0f, 0.0f);

            //bottom right
            vertices[5].position = new Vector3(right, bottom, 0.0f);
            vertices[5].texture = new Vector2(1.0f, 1.0f);

            DataStream vertStream = new DataStream(Marshal.SizeOf(typeof(Vertex2D)) * _vertexCount, true, true);
            foreach (Vertex2D vert in vertices)
            {
                vertStream.Write(vert);
            }
            vertStream.Position = 0;

            //clear the buffer if one exists
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Dispose();
            }
            _vertexBuffer = new Buffer(_device, vertStream, vertBufferDisc);

            vertStream.Close();
            vertStream.Dispose();
        }

        /// <summary>
        /// Load the texture from the given file
        /// </summary>
        /// <param name="fileName">The file to load from</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        private bool LoadTexture(string fileName)
        {
            bool result;
            _texture = new Texture(fileName, "");
            result = _texture.Initialize(_device);

            return result;
        }

        /// <summary>
        /// Get the texture used by the bitmap
        /// </summary>
        /// <returns>Returns the ShaderResourceView containing the texture</returns>
        public ShaderResourceView GetTexture()
        {
            return _texture.GetTexture();
        }

        /// <summary>
        /// Get the index count for the bitmap
        /// </summary>
        /// <returns>Returns the index count as an integer value</returns>
        public int GetIndexCount()
        {
            return _indexCount;
        }

        /// <summary>
        /// Get the vertex count for the bitmap(should return 6, if not something's probably wrong)
        /// </summary>
        /// <returns>Returns the vertex count as an integer value</returns>
        public int GetVertexCount()
        {
            return _vertexCount;
        }

        /// <summary>
        /// Draw the bitmap using the given device context and positions
        /// </summary>
        /// <param name="context">The device context</param>
        /// <param name="positionX">The x position to draw at</param>
        /// <param name="positionY">The y position to draw at</param>
        public void Draw(DeviceContext context, int positionX, int positionY)
        {
            SetVertexBuffer(positionX, positionY);

            int stride = Marshal.SizeOf(typeof(Vertex2D));
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, stride, 0));
            context.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public void Dispose()
        {
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            _texture.Dispose();
        }
    }

    /// <summary>
    /// Defines a 2D vertex with both position and texture coordinates
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex2D
    {
        public Vector3 position;
        public Vector2 texture;
    }
}
