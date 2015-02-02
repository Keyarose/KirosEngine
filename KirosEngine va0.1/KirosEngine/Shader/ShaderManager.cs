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

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.Shader
{
    /// <summary>
    /// Manages all shader instances to prevent duplicates and provide access to already loaded shaders.
    /// </summary>
    class ShaderManager
    {
        private static ShaderManager _instance;
        private Device _device;
        private DeviceContext _context;
        private Dictionary<string, BaseShader> _shaders;
        protected string _shaderPath;

        private ShaderManager()
        {
            _shaders = new Dictionary<string, BaseShader>();
        }

        /// <summary>
        /// Public accessor for the Shader Manager's instance
        /// </summary>
        public static ShaderManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new ShaderManager();
                }
                return _instance;
            }
        }

        public string ShaderPath
        {
            get
            {
                return _shaderPath;
            }
        }

        public void SetShaderPath(string path)
        {
            _shaderPath = path;
        }

        /// <summary>
        /// Init the manager
        /// </summary>
        /// <param name="device">The directx device</param>
        public void Init(Device device, DeviceContext context)
        {
            _device = device;
            _context = context;
        }

        /// <summary>
        /// Add a new shader
        /// </summary>
        /// <param name="sceneShader">the shader program</param>
        /// <param name="key">the key to identify the shader</param>
        /// <returns>true if successful, false if the key is in use</returns>
        public bool AddShader(BaseShader sceneShader, string key)
        {
            if (!_shaders.ContainsKey(key))
            {
                _shaders.Add(key, sceneShader);
                return true;
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed to add scene shader, key: {0} is already in use.", key));
                return false;
            }
        }

        /// <summary>
        /// Add a shader by file names
        /// </summary>
        /// <param name="vertFile">the vertex shader</param>
        /// <param name="pixelFile">the pixel shader</param>
        /// <param name="vertMethod">the vertex method</param>
        /// <param name="pixelMethod">the pixel method</param>
        /// <param name="type">the type of shader</param>
        /// <param name="key">the identifying key</param>
        /// <returns>true if successful, false if the key is in use or the shader fails to intialize</returns>
        public bool AddShader(string vertFile, string pixelFile, string vertMethod, string pixelMethod, string key, ShaderBufferFlags bufferFlags, params InputElement[] elements)
        {
            if (_shaders.ContainsKey(key))
            {
                ErrorLogger.Write(String.Format("Failed to add scene shader, key: {0} is already in use.", key));
                return false;
            }
            else
            {
                BaseShader newShader = new BaseShader();
                bool result = newShader.Initialize(_device, _context, vertFile, pixelFile, vertMethod, pixelMethod, bufferFlags, elements);
                _shaders.Add(key, newShader);
                return result;
            }
        }

        /// <summary>
        /// Get the shader for the given key
        /// </summary>
        /// <param name="key">the given key</param>
        /// <returns>the base shader registered for the key or null if there is none</returns>
        public BaseShader GetShaderForKey(string key)
        {
            if (_shaders.ContainsKey(key))
            {
                return _shaders[key];
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed get scene shader, key: {0} is not registered.", key));
                return null;
            }
        }
    }
}
