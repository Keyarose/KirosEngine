using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using KirosEngine.Scene;
using KirosEngine.Camera;
using KirosEngine.Light;
using KirosEngine.Material;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Model
{
    abstract class BaseModel : SceneNode
    {
        [StructLayout(LayoutKind.Sequential)]
        struct ColorVertex
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

        /// <summary>
        /// Public accessor for the model's vertex count
        /// </summary>
        public int VertexCount
        {
            get
            {
                return _vertexCount;
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

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="position">The position of the model</param>
        public BaseModel(Device device, Vector3 position) : base("default", position)
        {
            _device = device;
        }

        /// <summary>
        /// Constructor with nodeID specified
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="position">The position of the model</param>
        /// <param name="nodeID">The ID string for the node</param>
        public BaseModel(Device device, Vector3 position, string nodeID) : base(nodeID, position)
        {
            _device = device;
        }

        /// <summary>
        /// Constructor with Material
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="position">The position of the model</param>
        /// <param name="nodeID">The ID string for the node</param>
        /// <param name="material">The material for the model</param>
        public BaseModel(Device device, Vector3 position, string nodeID, ObjectMaterial material) : base(nodeID, position)
        {
            _device = device;
            _material = material;
        }

        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="context">The device context</param>
        public virtual void Draw(DeviceContext context, Matrix worldMatrix, Matrix projectionMatrix, Matrix viewMatrix, BaseCamera camera, params BasicLight[] lights)
        {
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, this._vertexStride, 0));
            context.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            if(_material == null)
            {
                ErrorLogger.Write(String.Format("Attempting to draw model: {0} with no material set", this._nodeID));
                //throw exception
            }
            _material.Draw(context, worldMatrix, projectionMatrix, viewMatrix, this.IndexCount, this.Position, camera, lights);
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
