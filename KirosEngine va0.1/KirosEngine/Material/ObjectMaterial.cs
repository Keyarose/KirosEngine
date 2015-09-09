using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using KirosEngine.Camera;
using KirosEngine.Light;
using KirosEngine.Model;
using KirosEngine.Textures;
using KirosEngine.Shader;

namespace KirosEngine.Material
{
    class ObjectMaterial
    {
        protected string _name;
        protected string _shaderID;
        protected string _texture;

        protected Vector4 _color;

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
        
        /// <summary>
        /// Create a material with a name, texture, and shader
        /// </summary>
        /// <param name="name">The material name</param>
        /// <param name="texture">The material texture</param>
        /// <param name="shader">The material shader</param>
        public ObjectMaterial(string name, string texture, string shader)
        {
            _name = name;
            _texture = texture;
            _shaderID = shader;
        }

        /// <summary>
        /// Create a material with a name, shader, and color
        /// </summary>
        /// <param name="name">The material name</param>
        /// <param name="shader">The material shader</param>
        /// <param name="color">The material color</param>
        public ObjectMaterial(string name, string shader, Vector4 color)
        {
            _name = name;
            _texture = null;
            _shaderID = shader;
            _color = color;
        }

        /// <summary>
        /// Create a material with a name, texture, shader, and color
        /// </summary>
        /// <param name="name">The material name</param>
        /// <param name="texture">The material texture</param>
        /// <param name="shader">The material shader</param>
        /// <param name="color">The material color</param>
        public ObjectMaterial(string name, string texture, string shader, Vector4 color)
        {
            _name = name;
            _texture = texture;
            _shaderID = shader;
            _color = color;
        }

        /// <summary>
        /// Draw using the settings defined by the material
        /// </summary>
        /// <param name="context">The device context</param>
        /// <param name="worldMatrix">The world matrix</param>
        /// <param name="projectionMatrix">The projection matrix</param>
        /// <param name="viewMatrix">The view matrix</param>
        /// <param name="model">The model being drawn</param>
        /// <param name="camera">The camera currently in use</param>
        /// <param name="lights">The lights to be used for the draw</param>
        public void Draw(DeviceContext context, Matrix worldMatrix, Matrix projectionMatrix, Matrix viewMatrix, int indexCount, Vector3 position, BaseCamera camera, params BasicLight[] lights)
        {
            BaseShader shaderToUse = ShaderManager.Instance.GetShaderForKey(_shaderID);
            Texture texture = TextureManager.Instance.GetTexture(_texture);

            if(shaderToUse != null)
            {
                if (shaderToUse.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.LightBuffer | ShaderBufferFlags.CameraBuffer | ShaderBufferFlags.SamplerBuffer))
                {
                    shaderToUse.Draw(context, indexCount, Matrix.Translation(position) * worldMatrix, projectionMatrix, viewMatrix, texture.GetTexture(), lights[0], camera.Position);
                }
                else if (shaderToUse.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer | ShaderBufferFlags.PixelBuffer))
                {
                    shaderToUse.Draw(context, indexCount, Matrix.Translation(position) * worldMatrix, projectionMatrix, viewMatrix, texture.GetTexture(), _color);
                }
                else if (shaderToUse.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.LightBuffer | ShaderBufferFlags.CameraBuffer))
                {
                    shaderToUse.Draw(context, indexCount, Matrix.Translation(position) * worldMatrix, projectionMatrix, viewMatrix, lights[0], camera.Position);
                }
                else if (shaderToUse.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer))
                {
                    shaderToUse.Draw(context, indexCount, Matrix.Translation(position) * worldMatrix, projectionMatrix, viewMatrix, texture.GetTexture());
                }
                else if (shaderToUse.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer))
                {
                    shaderToUse.Draw(context, indexCount, Matrix.Translation(position) * worldMatrix, projectionMatrix, viewMatrix);
                }
            }
            else
            {
                ErrorLogger.Write(String.Format("Object Material: {0} is not properly initialized. The given shader ID: {1}, is not recognized.", _name, _shaderID));
            }
        }
    }
}
