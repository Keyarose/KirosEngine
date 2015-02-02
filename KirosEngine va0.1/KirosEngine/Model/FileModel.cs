using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    class FileModel : BaseModel
    {
        [StructLayout(LayoutKind.Sequential)]
        struct TexVertex
        {
            public Vector3 position;
            public Vector2 texture;
            public Vector3 normal;
        };

        struct ModelType
        {
            public float x, y, z;
            public float tu, tv;
            public float nx, ny, nz;
        };

        Texture _texture;
        private string _modelFileName;
        private ModelType[] _model;

        public FileModel(Device device, string modelFileName, string textureFileName) : base(device)
        {
            _vertexStride = Marshal.SizeOf(typeof(TexVertex));
            _modelFileName = modelFileName;

            LoadModel(modelFileName);
            LoadTexture(textureFileName);
            Init();
        }

        public FileModel(Device device, string modelFileName, Texture texture) : base(device)
        {
            _vertexStride = Marshal.SizeOf(typeof(TexVertex));
            _modelFileName = modelFileName;
            _texture = texture;
            texture.Initialize(device);

            LoadModel(modelFileName);
            Init();
        }

        private void Init()
        {
            TexVertex[] vertices = new TexVertex[_vertexCount];
            ulong[] indices = new ulong[_indexCount];

            for (int i = 0; i < _vertexCount; i++)
            {
                vertices[i].position = new Vector3(_model[i].x, _model[i].y, _model[i].z);
                vertices[i].texture = new Vector2(_model[i].tu, _model[i].tv);
                vertices[i].normal = new Vector3(_model[i].nx, _model[i].ny, _model[i].nz);

                indices[i] = (ulong)i;
            }

            _verticeStream = new DataStream(_vertexStride * _vertexCount, true, true);
            foreach (TexVertex vertex in vertices)
            {
                _verticeStream.Write(vertex);
            }
            _verticeStream.Position = 0;

            _vertexBuffer = new Buffer(_device, _verticeStream, _vertexStride * _vertexCount, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _verticeStream.Close();
            _verticeStream.Dispose();

            _indexStream = new DataStream(sizeof(uint) * _indexCount, true, true);
            foreach (uint index in indices)
            {
                _indexStream.Write(index);
            }
            _indexStream.Position = 0;

            _indexBuffer = new Buffer(_device, _indexStream, sizeof(uint) * _indexCount, ResourceUsage.Default, BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _indexStream.Close();
            _indexStream.Dispose();
        }

        private bool LoadModel(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            //get all metadata
            foreach (string line in lines)
            {
                //get the vertex count
                if (line.StartsWith("//VertexCount"))
                {
                    _vertexCount = int.Parse(line.Substring(13));
                    _indexCount = _vertexCount;
                }
            }

            _model = new ModelType[_vertexCount];
            int index = 0;

            //process each vertex
            foreach (string line in lines)
            {
                if (!line.StartsWith("//"))
                {
                    string temp = line.TrimStart(' ');
                    temp = temp.Replace("  ", " ");

                    string[] split = temp.Split(' ');

                    _model[index].x = float.Parse(split[0]);
                    _model[index].y = float.Parse(split[1]);
                    _model[index].z = float.Parse(split[2]);

                    _model[index].tu = float.Parse(split[3]);
                    _model[index].tv = float.Parse(split[4]);

                    _model[index].nx = float.Parse(split[5]);
                    _model[index].ny = float.Parse(split[6]);
                    _model[index].nz = float.Parse(split[7]);

                    index++;
                }
            }

            return true;
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
            _model = null;
            _texture.Dispose();
            base.Dispose();
        }
    }
}
