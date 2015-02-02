using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using KirosEngine.Material;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Model
{
    abstract class BaseModel
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public Vector3 Position;
            public Color4 Color;
        };

        protected ObjectMaterial _material;

        protected int _vertexStride;
        protected Device _device;

        protected Buffer _vertexBuffer;
        protected Buffer _indexBuffer;

        protected DataStream _verticeStream;
        protected DataStream _indexStream;

        protected int _vertexCount;
        protected int _indexCount;

        protected Vector3 _position;

        /// <summary>
        /// Public accessor for the model's position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Public accessor for the model's index count
        /// </summary>
        public int IndexCount
        {
            get
            {
                return _indexCount;
            }
        }

        /// <summary>
        /// Public accessor for the model's material
        /// </summary>
        public ObjectMaterial Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;
            }
        }

        public BaseModel(Device device)
        {
            _device = device;
        }

        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="context">The device context</param>
        public virtual void Draw(DeviceContext context)
        {
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, this._vertexStride, 0));
            context.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        /// <summary>
        /// Dispose of resources used
        /// </summary>
        public virtual void Dispose()
        {
            _indexBuffer.Dispose();

            _vertexBuffer.Dispose();
        }
    }
}
