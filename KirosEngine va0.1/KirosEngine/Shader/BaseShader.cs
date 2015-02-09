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
using KirosEngine.Light;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Shader
{
    enum ShaderTypeFlags
    {
        ColorShaderNoLight,
        TextureShaderNoLight,
        ColorShaderLight,
        TextureShaderLight,
        FontShader
    }

    [Flags]
    public enum ShaderBufferFlags
    {
        None = 0,
        MatrixBuffer = 1 << 0,
        SamplerBuffer = 1 << 1,
        LightBuffer = 1 << 2,
        CameraBuffer = 1 << 3,
        PixelBuffer = 1 << 4
    }

    class BaseShader : IDisposable
    {
        Device _device;
        DeviceContext _context;

        private ShaderBufferFlags _bufferFlags;
        private VertexShader _vertexShader;
        private PixelShader _pixelShader;
        private InputLayout _layout;
        private Dictionary<string, BufferDescription> _bufferDescriptions;
        private Buffer _matrixBuffer;
        private BufferDescription _matrixBufferDisc;
        private SamplerState _sampleState;
        private SamplerDescription _sampleDisc;
        private Buffer _lightBuffer;
        private BufferDescription _lightBufferDisc;
        private Buffer _cameraBuffer;
        private BufferDescription _cameraBufferDisc;
        private Buffer _pixelBuffer;
        private BufferDescription _pixelBufferDisc;

        /// <summary>
        /// Public accessor for the shader's buffer flags
        /// </summary>
        public ShaderBufferFlags ShaderBufferFlags
        {
            get
            {
                return _bufferFlags;
            }
        }

        #region XMLDictionaries
        public static Dictionary<string, Format> FormatDictionary = new Dictionary<string, Format> {
            {"R32_Float", Format.R32_Float},
            {"R32G32_Float", Format.R32G32_Float},
            {"R32G32B32_Float", Format.R32G32B32_Float},
            {"R32G32B32A32_Float", Format.R32G32B32A32_Float}
        };

        public static Dictionary<string, int> OffsetDictionary = new Dictionary<string, int> {
            {"0", 0},
            {"appendaligned", InputElement.AppendAligned}
        };

        public static Dictionary<string, InputClassification> InputDictionary = new Dictionary<string, InputClassification> {
            {"perinstancedata", InputClassification.PerInstanceData},
            {"pervertexdata", InputClassification.PerVertexData},
            {"PerInstanceData", InputClassification.PerInstanceData},
            {"PerVertexData", InputClassification.PerVertexData}
        };

        public static Dictionary<string, Filter> FilterDictionary = new Dictionary<string, Filter> {
            {"Anisotropic", Filter.Anisotropic},
            {"ComparisonAnisotropic", Filter.ComparisonAnisotropic},
            {"ComparisonMinLinearMagMipPoint", Filter.ComparisonMinLinearMagMipPoint},
            {"ComparisonMinLinearMagPointMipLinear", Filter.ComparisonMinLinearMagPointMipLinear},
            {"ComparisonMinMagLinearMipPoint", Filter.ComparisonMinMagLinearMipPoint},
            {"ComparisonMinMagMipLinear", Filter.ComparisonMinMagMipLinear},
            {"ComparisonMinMagMipPoint", Filter.ComparisonMinMagMipPoint},
            {"ComparisonMinMagPointMipLinear", Filter.ComparisonMinMagPointMipLinear},
            {"ComparisonMinPointMagLinearMipPoint", Filter.ComparisonMinPointMagLinearMipPoint},
            {"ComparisonMinPointMagMipLinear", Filter.ComparisonMinPointMagMipLinear},
            {"MinLinearMagMipPoint", Filter.MinLinearMagMipPoint},
            {"MinLinearMagPointMipLinear", Filter.MinLinearMagPointMipLinear},
            {"MinMagLinearMipPoint", Filter.MinMagLinearMipPoint},
            {"MinMagMipLinear", Filter.MinMagMipLinear},
            {"MinMagMipPoint", Filter.MinMagMipPoint},
            {"MinMagPointMipLinear", Filter.MinMagPointMipLinear},
            {"MinPointMagLinearMipPoint", Filter.MinPointMagLinearMipPoint},
            {"MinPointMagMipLinear", Filter.MinPointMagMipLinear}
        };

        public static Dictionary<string, TextureAddressMode> TextureAddressDictionary = new Dictionary<string, TextureAddressMode> {
            {"Border", TextureAddressMode.Border},
            {"Clamp", TextureAddressMode.Clamp},
            {"Mirror", TextureAddressMode.Mirror},
            {"MirrorOnce", TextureAddressMode.MirrorOnce},
            {"Wrap", TextureAddressMode.Wrap}
        };

        public static Dictionary<string, Comparison> ComparisonDictionary = new Dictionary<string, Comparison> {
            {"Always", Comparison.Always},
            {"Equal", Comparison.Equal},
            {"Greater", Comparison.Greater},
            {"GreaterEqual", Comparison.GreaterEqual},
            {"Less", Comparison.Less},
            {"LessEqual", Comparison.LessEqual},
            {"Never", Comparison.Never},
            {"NotEqual", Comparison.NotEqual}
        };

        public static Dictionary<string, ResourceUsage> ResourceUsageDictionary = new Dictionary<string,ResourceUsage> {
            {"Default", ResourceUsage.Default},
            {"Dynamic", ResourceUsage.Dynamic},
            {"Immutable", ResourceUsage.Immutable},
            {"Staging", ResourceUsage.Staging}
        };

        public static Dictionary<string, BindFlags> BindFlagsDictionary = new Dictionary<string,BindFlags> {
            {"ConstantBuffer", BindFlags.ConstantBuffer},
            {"DepthStencil", BindFlags.DepthStencil},
            {"IndexBuffer", BindFlags.IndexBuffer},
            {"None", BindFlags.None},
            {"RenderTarget", BindFlags.RenderTarget},
            {"ShaderResource", BindFlags.ShaderResource},
            {"StreamOutput", BindFlags.StreamOutput},
            {"UnorderedAccess", BindFlags.UnorderedAccess},
            {"VertexBuffer", BindFlags.VertexBuffer}
        };

        public static Dictionary<string, CpuAccessFlags> CpuAccessFlagsDictionary = new Dictionary<string,CpuAccessFlags> {
            {"None", CpuAccessFlags.None},
            {"Read", CpuAccessFlags.Read},
            {"Write", CpuAccessFlags.Write}
        };

        public static Dictionary<string, ResourceOptionFlags> ResourceOptionFlagsDictionary = new Dictionary<string,ResourceOptionFlags> {
            {"ClampedResource", ResourceOptionFlags.ClampedResource},
            {"DrawIndirect", ResourceOptionFlags.DrawIndirect},
            {"GdiCompatible", ResourceOptionFlags.GdiCompatible},
            {"GenerateMipMaps", ResourceOptionFlags.GenerateMipMaps},
            {"KeyedMutex", ResourceOptionFlags.KeyedMutex},
            {"None", ResourceOptionFlags.None},
            {"RawBuffer", ResourceOptionFlags.RawBuffer},
            {"Shared", ResourceOptionFlags.Shared},
            {"StructuredBuffer", ResourceOptionFlags.StructuredBuffer},
            {"TextureCube", ResourceOptionFlags.TextureCube}
        };
        #endregion

        public bool Initialize(Device device, DeviceContext context, string vertexShaderFile, string pixelShaderFile, string vertexMethod, string pixelMethod, ShaderBufferFlags bufferFlags, params InputElement[] elements)
        {
            _device = device;
            _context = context;
            _bufferDescriptions = new Dictionary<string, BufferDescription>();
            _bufferFlags = bufferFlags;
            ShaderSignature inputSignature = null;

            try
            {
                using(var bytecode = ShaderBytecode.CompileFromFile(vertexShaderFile, vertexMethod, "vs_4_0", ShaderFlags.None, EffectFlags.None))
                {
                    inputSignature = ShaderSignature.GetInputSignature(bytecode);
                    _vertexShader = new VertexShader(device, bytecode);
                    bytecode.Dispose();
                }

                using(var bytecode = ShaderBytecode.CompileFromFile(pixelShaderFile, pixelMethod, "ps_4_0", ShaderFlags.None, EffectFlags.None))
                {
                    _pixelShader = new PixelShader(device, bytecode);
                    bytecode.Dispose();
                }
            }
            catch(CompilationException ex)
            {
                System.Console.WriteLine("Shader Compilation Failed. Message: {0}", ex.Message);
                ErrorLogger.Write(string.Format("Shader Compilation Failed. Message: {0}", ex.Message));

                //dispose of the inputsignature if it was created before returning
                if (inputSignature != null)
                {
                    inputSignature.Dispose();
                }

                return false;
            }

            _layout = new InputLayout(_device, inputSignature, elements);

            if(bufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer))
            {
                _matrixBufferDisc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Marshal.SizeOf(typeof(MatrixBufferType)),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };
            }

            //if the shader uses a texture
            if (bufferFlags.HasFlag(ShaderBufferFlags.SamplerBuffer))
            {
                _sampleDisc = new SamplerDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0.0f,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };

                _sampleState = SamplerState.FromDescription(device, _sampleDisc);
            }

            //if the shader uses light
            if (bufferFlags.HasFlag(ShaderBufferFlags.LightBuffer))
            {
                _lightBufferDisc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Marshal.SizeOf(typeof(LightBufferType)),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };
            }

            if(bufferFlags.HasFlag(ShaderBufferFlags.CameraBuffer))
            {
                 _cameraBufferDisc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Marshal.SizeOf(typeof(CameraBufferType)),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };
            }

            //if the shader uses pixel buffer
            if (bufferFlags.HasFlag(ShaderBufferFlags.PixelBuffer))
            {
                _pixelBufferDisc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Marshal.SizeOf(typeof(PixelBufferType)),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };
            }

            return true;
        }

        /// <summary>
        /// Add a buffer description with the identifying string
        /// </summary>
        /// <param name="id">The id string</param>
        /// <param name="descript">The buffer description</param>
        /// <returns>True if successful, false if id is already in use</returns>
        public bool AddBufferDescription(string id, BufferDescription descript)
        {
            if(_bufferDescriptions.ContainsKey(id))
            {
                return false;
            }

            _bufferDescriptions.Add(id, descript);

            return true;
        }

        /// <summary>
        /// Change the buffer description for the given id
        /// </summary>
        /// <param name="id">The id string</param>
        /// <param name="descript">The buffer description</param>
        public void ChangeBufferDescription(string id, BufferDescription descript)
        {
            if(_bufferDescriptions.ContainsKey(id))
            {
                _bufferDescriptions[id] = descript;
            }
            else
            {
                //throw key not in use warning
                ErrorLogger.Write(string.Format("Error changing registered buffer description for shader: {0}, with key: {1}. Key is not in use", this, id));
            }
        }

        /// <summary>
        /// Set the shader's sampler discription for sampling textures
        /// </summary>
        /// <param name="discript">The description for the sampler</param>
        /// <returns>true if successful</returns>
        public bool SetSamplerDiscription(SamplerDescription discript)
        {
            _sampleDisc = discript;

            _sampleState = SamplerState.FromDescription(_device, _sampleDisc);

            return true;
        }

        /// <summary>
        /// Set the shader's sampler discription for sampling textures
        /// </summary>
        /// <param name="filter">The filtering method to use</param>
        /// <param name="addressU">method to use for resolving a texture coordinate outside 0 to 1</param>
        /// <param name="addressV">method to use for resolving a texture coordinate outside 0 to 1</param>
        /// <param name="addressW">method to use for resolving a texture coordinate outside 0 to 1</param>
        /// <param name="lodBias">offset from the calculated mip level</param>
        /// <param name="maxAnisotropy">clamp value for anisotropic filtering</param>
        /// <param name="compairFunction">fucntion to compare the sampled data</param>
        /// <param name="border">color to use if using border addressing</param>
        /// <param name="minLod">lowest end of the mipmap range to clamp access to</param>
        /// <param name="maxLod">highest end of the mipmap range to clamp access to</param>
        /// <returns>true if successful</returns>
        public bool SetSamplerDiscription(Filter filter, TextureAddressMode addressU, TextureAddressMode addressV, TextureAddressMode addressW, float lodBias,
             Color4 border, int maxAnisotropy = 1, Comparison compairFunction = Comparison.Always, float minLod = 0.0f, float maxLod = float.MaxValue)
        {
            _sampleDisc = new SamplerDescription()
            {
                Filter = filter,
                AddressU = addressU,
                AddressV = addressV,
                AddressW = addressW,
                MipLodBias = lodBias,
                MaximumAnisotropy = maxAnisotropy,
                ComparisonFunction = compairFunction,
                BorderColor = border,
                MinimumLod = minLod,
                MaximumLod = maxLod
            };

            _sampleState = SamplerState.FromDescription(_device, _sampleDisc);

            return true;
        }

        /// <summary>
        /// Set the constant buffer with a matrix
        /// </summary>
        /// <param name="vertexShader">If true the constant is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="bufferID">The id for the buffer discription used</param>
        /// <param name="slot">The constant's order in the shader</param>
        /// <param name="matrix">The matrix to put into the buffer</param>
        /// <returns>True if successful, false if it fails</returns>
        public bool SetConstant(bool vertexShader, string bufferID, int slot, Matrix matrix)
        {
            DataStream matrixStream = new DataStream(Marshal.SizeOf(matrix.GetType()), true, true);
            matrixStream.Write(matrix);
            matrixStream.Position = 0;

            //check to see if the description given by the id exists
            BufferDescription descript;
            if(_bufferDescriptions.ContainsKey(bufferID))
            {
                descript = _bufferDescriptions[bufferID];
                descript.SizeInBytes = Marshal.SizeOf(matrix.GetType());
            }
            else
            {
                return false;
            }

            Buffer matrixBuffer = new Buffer(_device, matrixStream, _bufferDescriptions[bufferID]);
            matrixStream.Close();
            matrixStream.Dispose();

            if(vertexShader)
            {
                _context.VertexShader.SetConstantBuffer(matrixBuffer, slot);
            }
            else
            {
                _context.PixelShader.SetConstantBuffer(matrixBuffer, slot);
            }

            matrixBuffer.Dispose();
            return true;
        }

        /// <summary>
        /// Set the constant buffer with a vector3
        /// </summary>
        /// <param name="vertexShader">If true the constant is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="bufferDescr">The discription used to create the buffer</param>
        /// <param name="slot">The constant's order in the shader</param>
        /// <param name="vec3">The vector3 to put into the buffer</param>
        /// <returns>True if successful, false if it fails</returns>
        public bool SetConstant(bool vertexShader, string bufferID, int slot, Vector3 vec3)
        {
            DataStream vecStream = new DataStream(Marshal.SizeOf(vec3.GetType()), true, true);
            vecStream.Write(vec3);
            vecStream.Position = 0;

            //check to see if the description given by the id exists
            BufferDescription descript;
            if(_bufferDescriptions.ContainsKey(bufferID))
            {
                descript = _bufferDescriptions[bufferID];
                descript.SizeInBytes = Marshal.SizeOf(vec3.GetType());
            }
            else
            {
                return false;
            }

            Buffer vecBuffer = new Buffer(_device, vecStream, descript);
            vecStream.Close();
            vecStream.Dispose();

            if(vertexShader)
            {
                _context.VertexShader.SetConstantBuffer(vecBuffer, slot);
            }
            else
            {
                _context.PixelShader.SetConstantBuffer(vecBuffer, slot);
            }

            vecBuffer.Dispose();
            return true;
        }

        /// <summary>
        /// Set the constant buffer with a float
        /// </summary>
        /// <param name="vertexShader">If true the constant is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="bufferDescr">The discription used to create the buffer</param>
        /// <param name="slot">The constant's order in the shader</param>
        /// <param name="valF">The float to put into the buffer</param>
        /// <returns>True if successful, false if it fails</returns>
        public bool SetConstant(bool vertexShader, string bufferID, int slot, float valF)
        {
            DataStream floatStream = new DataStream(Marshal.SizeOf(valF.GetType()), true, true);
            floatStream.Write(valF);
            floatStream.Position = 0;

            //check to see if the description given by the id exists
            BufferDescription descript;
            if(_bufferDescriptions.ContainsKey(bufferID))
            {
                descript = _bufferDescriptions[bufferID];
                descript.SizeInBytes = Marshal.SizeOf(valF.GetType());
            }
            else
            {
                return false;
            }

            Buffer floatBuffer = new Buffer(_device, floatStream, descript);
            floatStream.Close();
            floatStream.Dispose();

            if(vertexShader)
            {
                _context.VertexShader.SetConstantBuffer(floatBuffer, slot);
            }
            else
            {
                _context.PixelShader.SetConstantBuffer(floatBuffer, slot);
            }

            floatBuffer.Dispose();
            return true;
        }

        /// <summary>
        /// Set the constant buffer with a set of matrixes
        /// </summary>
        /// <param name="vertexShader">If true the constant is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="bufferDescr">The discription used to create the buffer</param>
        /// <param name="slot">The constant's order in the shader</param>
        /// <param name="matrixBufferType">The buffers to put into the buffer</param>
        /// <returns>True if successful, false if ti fails</returns>
        public bool SetConstant(bool vertexShader, string bufferID, int slot, MatrixBufferType matrixBufferType)
        {
            DataStream matrixStream = new DataStream(Marshal.SizeOf(matrixBufferType.GetType()), true, true);
            matrixStream.Write(matrixBufferType);
            matrixStream.Position = 0;

            //check to see if the description given by the id exists
            BufferDescription descript;
            if(_bufferDescriptions.ContainsKey(bufferID))
            {
                descript = _bufferDescriptions[bufferID];
                descript.SizeInBytes = Marshal.SizeOf(matrixBufferType.GetType());
            }
            else
            {
                return false;
            }

            Buffer matrixBuffer = new Buffer(_device, matrixStream, descript);
            matrixStream.Close();
            matrixStream.Dispose();

            if(vertexShader)
            {
                _context.VertexShader.SetConstantBuffer(matrixBuffer, slot);
            }
            else
            {
                _context.PixelShader.SetConstantBuffer(matrixBuffer, slot);
            }

            matrixBuffer.Dispose();
            return true;
        }

        /// <summary>
        /// Set the constant buffer with a set of values for a light
        /// </summary>
        /// <param name="vertexShader">If true the constant is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="bufferDescr">The discription used to create the buffer</param>
        /// <param name="slot">The constant's order in the shader</param>
        /// <param name="lightBufferType">The light values to put into the buffer</param>
        public bool SetConstant(bool vertexShader, string bufferID, int slot, LightBufferType lightBufferType)
        {
            DataStream lightStream = new DataStream(Marshal.SizeOf(lightBufferType.GetType()), true, true);
            lightStream.Write(lightBufferType);
            lightStream.Position = 0;

            BufferDescription description;
            if(_bufferDescriptions.ContainsKey(bufferID))
            {
                description = _bufferDescriptions[bufferID];
                description.SizeInBytes = Marshal.SizeOf(lightBufferType.GetType());
            }
            else
            {
                return false;
            }

            Buffer lightBuffer = new Buffer(_device, lightStream, description);
            lightStream.Close();
            lightStream.Dispose();

            if(vertexShader)
            {
                _context.VertexShader.SetConstantBuffer(lightBuffer, slot);
            }
            else
            {
                _context.PixelShader.SetConstantBuffer(lightBuffer, slot);
            }

            lightBuffer.Dispose();
            return true;
        }

        /// <summary>
        /// Set a shader resource 
        /// </summary>
        /// <param name="vertexShader">If true the resource is ment for the vertex shader, if false it is ment for the pixel shader</param>
        /// <param name="slot">The resource's order in the shader</param>
        /// <param name="resourceView">The resource view to use</param>
        public void SetShaderResource(bool vertexShader, int slot, ShaderResourceView resourceView)
        {
            if (vertexShader)
            {
                _context.VertexShader.SetShaderResource(resourceView, slot);
            }
            else
            {
                _context.PixelShader.SetShaderResource(resourceView, slot);
            }
        }

        /// <summary>
        /// Set the paramiters of the shader for drawing
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="world">world matirx</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        private void SetParamiters(DeviceContext context, Matrix world, Matrix projection, Matrix view)
        {
            MatrixBufferType data;

            world = Matrix.Transpose(world);
            projection = Matrix.Transpose(projection);
            view = Matrix.Transpose(view);

            data.projection = projection;
            data.world = world;
            data.view = view;

            DataStream matrixStream = new DataStream(Marshal.SizeOf(typeof(MatrixBufferType)), true, true);
            matrixStream.Write(data);
            matrixStream.Position = 0;

            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }
            _matrixBuffer = new Buffer(_device, matrixStream, _matrixBufferDisc);

            matrixStream.Close();
            matrixStream.Dispose();

            context.VertexShader.SetConstantBuffer(_matrixBuffer, 0);
        }

        /// <summary>
        /// set the paramiters of the shader for drawing with a light
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="lightDirection">light direction</param>
        /// <param name="diffuseColor">light color</param>
        private void SetParamiters(DeviceContext context, Matrix world, Matrix projection, Matrix view, BasicLight light, Vector3 cameraPosition)
        {
            MatrixBufferType data;
            LightBufferType lightData;
            CameraBufferType cameraData;

            world = Matrix.Transpose(world);
            projection = Matrix.Transpose(projection);
            view = Matrix.Transpose(view);

            //matrix buffer
            data.projection = projection;
            data.world = world;
            data.view = view;

            DataStream matrixStream = new DataStream(Marshal.SizeOf(typeof(MatrixBufferType)), true, true);
            matrixStream.Write(data);
            matrixStream.Position = 0;

            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }
            _matrixBuffer = new Buffer(_device, matrixStream, _matrixBufferDisc);

            matrixStream.Close();
            matrixStream.Dispose();

            //camera buffer
            cameraData.cameraPosition = cameraPosition;
            cameraData.padding = 0.0f;

            DataStream cameraStream = new DataStream(Marshal.SizeOf(typeof(CameraBufferType)), true, true);
            cameraStream.Write(cameraData);
            cameraStream.Position = 0;

            if (_cameraBuffer != null)
            {
                _cameraBuffer.Dispose();
            }
            _cameraBuffer = new Buffer(_device, cameraStream, _cameraBufferDisc);

            cameraStream.Close();
            cameraStream.Dispose();

            //light buffer
            lightData.ambientColor = light.GetAmbientColor();
            lightData.diffuseColor = light.GetDiffuseColor();
            lightData.lightDirection = light.GetDirection();
            lightData.specularPower = light.GetSpecularPower();
            lightData.specularColor = light.GetSpecularColor();

            DataStream lightStream = new DataStream(Marshal.SizeOf(typeof(LightBufferType)), true, true);
            lightStream.Write(lightData);
            lightStream.Position = 0;

            if (_lightBuffer != null)
            {
                _lightBuffer.Dispose();
            }
            _lightBuffer = new Buffer(_device, lightStream, _lightBufferDisc);

            lightStream.Close();
            lightStream.Dispose();

            context.VertexShader.SetConstantBuffer(_matrixBuffer, 0);
            context.PixelShader.SetConstantBuffer(_lightBuffer, 0);
            context.VertexShader.SetConstantBuffer(_cameraBuffer, 1);
        }

        /// <summary>
        /// set the paramiters of the shader for drawing with texture
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture</param>
        private void SetParamiters(DeviceContext context, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture)
        {
            MatrixBufferType data;

            world = Matrix.Transpose(world);
            projection = Matrix.Transpose(projection);
            view = Matrix.Transpose(view);

            data.projection = projection;
            data.world = world;
            data.view = view;

            DataStream matrixStream = new DataStream(Marshal.SizeOf(typeof(MatrixBufferType)), true, true);
            matrixStream.Write(data);
            matrixStream.Position = 0;

            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }
            _matrixBuffer = new Buffer(_device, matrixStream, _matrixBufferDisc);

            matrixStream.Close();
            matrixStream.Dispose();

            context.VertexShader.SetConstantBuffer(_matrixBuffer, 0);
            context.PixelShader.SetShaderResource(texture, 0);
        }

        /// <summary>
        /// Set the shader paramiters for drawing with texture and color (used in text rendering)
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture resource</param>
        /// <param name="color">pixel color</param>
        private void SetParamiters(DeviceContext context, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture, Vector4 color)
        {
            MatrixBufferType data;
            PixelBufferType pixel;

            world = Matrix.Transpose(world);
            projection = Matrix.Transpose(projection);
            view = Matrix.Transpose(view);

            data.world = world;
            data.projection = projection;
            data.view = view;

            DataStream matrixStream = new DataStream(Marshal.SizeOf(typeof(MatrixBufferType)), true, true);
            matrixStream.Write(data);
            matrixStream.Position = 0;

            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }
            _matrixBuffer = new Buffer(_device, matrixStream, _matrixBufferDisc);

            matrixStream.Close();
            matrixStream.Dispose();

            //pixel buffer
            pixel.pixelColor = color;

            DataStream pixelStream = new DataStream(Marshal.SizeOf(typeof(PixelBufferType)), true, true);
            pixelStream.Write(pixel);
            pixelStream.Position = 0;

            if (_pixelBuffer != null)
            {
                _pixelBuffer.Dispose();
            }
            _pixelBuffer = new Buffer(_device, pixelStream, _pixelBufferDisc);

            pixelStream.Close();
            pixelStream.Dispose();

            context.VertexShader.SetConstantBuffer(_matrixBuffer, 0);
            context.PixelShader.SetShaderResource(texture, 0);
            context.PixelShader.SetConstantBuffer(_pixelBuffer, 0);
        }

        /// <summary>
        /// Set the paramiters of the shader for drawing with light data and texture
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture</param>
        /// <param name="lightDirection">light direction</param>
        /// <param name="diffuseColor">light color</param>
        private void SetParamiters(DeviceContext context, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture, BasicLight light, Vector3 cameraPosition)
        {
            MatrixBufferType data;
            LightBufferType lightData;
            CameraBufferType cameraData;

            world = Matrix.Transpose(world);
            projection = Matrix.Transpose(projection);
            view = Matrix.Transpose(view);

            //matrix buffer
            data.projection = projection;
            data.world = world;
            data.view = view;

            DataStream matrixStream = new DataStream(Marshal.SizeOf(typeof(MatrixBufferType)), true, true);
            matrixStream.Write(data);
            matrixStream.Position = 0;

            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }
            _matrixBuffer = new Buffer(_device, matrixStream, _matrixBufferDisc);

            matrixStream.Close();
            matrixStream.Dispose();

            //camera buffer
            cameraData.cameraPosition = cameraPosition;
            cameraData.padding = 0.0f;

            DataStream cameraStream = new DataStream(Marshal.SizeOf(typeof(CameraBufferType)), true, true);
            cameraStream.Write(cameraData);
            cameraStream.Position = 0;

            if (_cameraBuffer != null)
            {
                _cameraBuffer.Dispose();
            }
            _cameraBuffer = new Buffer(_device, cameraStream, _cameraBufferDisc);

            cameraStream.Close();
            cameraStream.Dispose();

            //light buffer
            lightData.ambientColor = light.GetAmbientColor();
            lightData.diffuseColor = light.GetDiffuseColor();
            lightData.lightDirection = light.GetDirection();
            lightData.specularPower = light.GetSpecularPower();
            lightData.specularColor = light.GetSpecularColor();

            DataStream lightStream = new DataStream(Marshal.SizeOf(typeof(LightBufferType)), true, true);
            lightStream.Write(lightData);
            lightStream.Position = 0;

            if (_lightBuffer != null)
            {
                _lightBuffer.Dispose();
            }
            _lightBuffer = new Buffer(_device, lightStream, _lightBufferDisc);

            lightStream.Close();
            lightStream.Dispose();

            context.VertexShader.SetConstantBuffer(_matrixBuffer, 0);
            context.PixelShader.SetShaderResource(texture, 0);
            context.PixelShader.SetConstantBuffer(_lightBuffer, 0);
            context.VertexShader.SetConstantBuffer(_cameraBuffer, 1);

            _matrixBuffer.Dispose();
            _lightBuffer.Dispose();
            _cameraBuffer.Dispose();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="indexCount">index count</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        public void Draw(DeviceContext context, int indexCount, Matrix world, Matrix projection, Matrix view)
        {
            SetParamiters(context, world, projection, view);

            context.InputAssembler.InputLayout = _layout;

            context.VertexShader.Set(_vertexShader);
            context.PixelShader.Set(_pixelShader);

            context.DrawIndexed(indexCount, 0, 0);
        }

        /// <summary>
        /// Draw with color and light data
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="indexCount">index count</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="lightDirection">light direction</param>
        /// <param name="diffuseColor">light color</param>
        public void Draw(DeviceContext context, int indexCount, Matrix world, Matrix projection, Matrix view, BasicLight light, Vector3 cameraPosition)
        {
            SetParamiters(context, world, projection, view, light, cameraPosition);

            context.InputAssembler.InputLayout = _layout;

            context.VertexShader.Set(_vertexShader);
            context.PixelShader.Set(_pixelShader);

            context.DrawIndexed(indexCount, 0, 0);
        }

        /// <summary>
        /// Draw with texture
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="indexCount">index count</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture</param>
        public void Draw(DeviceContext context, int indexCount, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture)
        {
            SetParamiters(context, world, projection, view, texture);

            context.InputAssembler.InputLayout = _layout;

            context.VertexShader.Set(_vertexShader);
            context.PixelShader.Set(_pixelShader);

            context.PixelShader.SetSampler(_sampleState, 0);

            context.DrawIndexed(indexCount, 0, 0);
        }

        /// <summary>
        /// Draw with a texture and a color (used in text rendering)
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="indexCount">object index count</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture resource</param>
        /// <param name="color">pixel color</param>
        public void Draw(DeviceContext context, int indexCount, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture, Vector4 color)
        {
            SetParamiters(context, world, projection, view, texture, color);

            context.InputAssembler.InputLayout = _layout;

            context.VertexShader.Set(_vertexShader);
            context.PixelShader.Set(_pixelShader);

            context.PixelShader.SetSampler(_sampleState, 0);

            context.DrawIndexed(indexCount, 0, 0);
        }

        /// <summary>
        /// Draw with texture and light data
        /// </summary>
        /// <param name="context">device context</param>
        /// <param name="indexCount">index count</param>
        /// <param name="world">world matrix</param>
        /// <param name="projection">projection matrix</param>
        /// <param name="view">view matrix</param>
        /// <param name="texture">texture</param>
        /// <param name="lightDirection">light direction</param>
        /// <param name="diffuseColor">light color</param>
        public void Draw(DeviceContext context, int indexCount, Matrix world, Matrix projection, Matrix view, ShaderResourceView texture, BasicLight light, Vector3 cameraPosition)
        {
            SetParamiters(context, world, projection, view, texture, light, cameraPosition);

            context.InputAssembler.InputLayout = _layout;

            context.VertexShader.Set(_vertexShader);
            context.PixelShader.Set(_pixelShader);

            context.PixelShader.SetSampler(_sampleState, 0);

            context.DrawIndexed(indexCount, 0, 0);
        }

        /// <summary>
        /// Dispose of the resources
        /// </summary>
        public void Dispose()
        {
            if (_matrixBuffer != null)
            {
                _matrixBuffer.Dispose();
            }

            if (_lightBuffer != null)
            {
                _lightBuffer.Dispose();
            }

            if (_pixelBuffer != null)
            {
                _pixelBuffer.Dispose();
            }

            if (_cameraBuffer != null)
            {
                _cameraBuffer.Dispose();
            }

            if (_layout != null)
            {
                _layout.Dispose();
            }

            if (_pixelShader != null)
            {
                _pixelShader.Dispose();
            }

            if (_vertexShader != null)
            {
                _vertexShader.Dispose();
            }

            if (_sampleState != null)
            {
                _sampleState.Dispose();
            }
        }
    }
}
