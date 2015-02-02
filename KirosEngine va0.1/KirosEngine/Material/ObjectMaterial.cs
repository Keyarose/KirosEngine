using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using KirosEngine.Textures;
using KirosEngine.Shader;

namespace KirosEngine.Material
{
    class ObjectMaterial
    {
        protected string _name;
        protected string _shaderID;
        protected string _texture;

        /// <summary>
        /// Public accessor for the Material's name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Public accessor for the Material's shader id
        /// </summary>
        public string ShaderID
        {
            get
            {
                return _shaderID;
            }
        }

        public string TextureID
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }

        public ObjectMaterial(string name, string texture, string shader)
        {
            _name = name;
            _texture = texture;
            _shaderID = shader;
        }

        public void Draw(DeviceContext context)
        {
            BaseShader shaderToUse = ShaderManager.Instance.GetShaderForKey(_shaderID);

            if(shaderToUse != null)
            {
                
            }
        }
    }
}
