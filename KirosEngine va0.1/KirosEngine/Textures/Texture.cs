using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

using Device = SlimDX.Direct3D11.Device;

namespace KirosEngine.Textures
{
    /// <summary>
    /// Defines a 2D texture resource
    /// </summary>
    class Texture : IDisposable
    {
        //TODO: exception and error checking
        ShaderResourceView _texture;
        protected string _id;
        protected string _fileName;

        public string TextureID
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Public accessor to the texture's ShaderResourceView
        /// </summary>
        /// <returns>The ShaderResourceView for the texture</returns>
        public ShaderResourceView GetTexture()
        {
            return _texture;
        }

        public Texture(string fileName, string id)
        {
            _fileName = fileName;
            _id = id;
        }

        /// <summary>
        /// Initalize the texture using the given file
        /// </summary>
        /// <param name="device">The DirectX device</param>
        /// <param name="fileName">The file to load from</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool Initialize(Device device)
        {
            Texture2D tex2D = null;

            try
            {
                tex2D = Texture2D.FromFile(device, _fileName);
            }
            catch (System.IO.IOException ex)
            {
                //issue getting the file this should be handled by the developer
                Console.WriteLine(ex.Message);
                return false;
            }

            _texture = new ShaderResourceView(device, tex2D);
            return true;
        }

        /// <summary>
        /// Get the width of the texture
        /// </summary>
        /// <returns>Integer representing the width of the texture</returns>
        public int GetTextureWidth()
        {
            var res = (Texture2D)_texture.Resource;
            return res.Description.Width;
        }

        /// <summary>
        /// Get the height of the texture
        /// </summary>
        /// <returns>Integer representing the height of the texture</returns>
        public int GetTextureHeight()
        {
            var res = (Texture2D)_texture.Resource;
            return res.Description.Height;
        }

        /// <summary>
        /// Dispose of the texture object
        /// </summary>
        public void Dispose()
        {
            _texture.Dispose();
        }
    }
}
