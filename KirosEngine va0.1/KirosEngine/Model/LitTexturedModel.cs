using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using KirosEngine.Textures;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Model
{
    class LitTexturedModel : BaseModel
    {
        [StructLayout(LayoutKind.Sequential)]
        struct TexNormVertex
        {
            public Vector3 Position;
            public Vector2 Texture;
            public Vector3 Normal;
        };

        private Texture _texture;

        public LitTexturedModel(Device device, string fileName) : base(device)
        {
            _vertexStride = Marshal.SizeOf(typeof(TexNormVertex));
            this.LoadTexture(fileName);
            this.Init();
        }

        private void Init()
        {
            TexNormVertex[] vertices = new TexNormVertex[_vertexCount];

            vertices[0].Position = new Vector3(-1.0f, -1.0f, 0.0f);
            vertices[1].Position = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[2].Position = new Vector3(1.0f, -1.0f, 0.0f);

            vertices[0].Texture = new Vector2(0.0f, 1.0f);
            vertices[1].Texture = new Vector2(0.5f, 0.0f);
            vertices[2].Texture = new Vector2(1.0f, 1.0f);

            vertices[0].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[1].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            vertices[2].Normal = new Vector3(0.0f, 0.0f, -1.0f);

            uint[] indices = new uint[_indexCount];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            _verticeStream = new DataStream(_vertexStride * _vertexCount, true, true);
            foreach (TexNormVertex vertex in vertices)
            {
                _verticeStream.Write(vertex);
            }
            _verticeStream.Position = 0;

            _vertexBuffer = new Buffer(_device, _verticeStream, _vertexStride * _vertexCount, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            _indexStream = new DataStream(sizeof(uint) * _indexCount, true, true);
            foreach (uint index in indices)
            {
                _indexStream.Write(index);
            }
            _indexStream.Position = 0;

            _indexBuffer = new Buffer(_device, _indexStream, sizeof(uint) * _indexCount, ResourceUsage.Default, BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        private bool LoadTexture(string fileName)
        {
            _texture = new Texture(fileName, "");
            _texture.Initialize(_device);

            return true;
        }

        public ShaderResourceView GetTexture()
        {
            return _texture.GetTexture();
        }

        public override void Dispose()
        {
            _texture.Dispose();
            base.Dispose();
        }
    }
}
