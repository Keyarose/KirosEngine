using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using KirosEngine.Scene;

namespace KirosEngine.Light
{
    /// <summary>
    /// Base class for all light objects
    /// </summary>
    class BasicLight : SceneNode
    {
        private Color4 _ambientColor;
        private Color4 _diffuseColor;
        private Vector3 _direction;
        private Color4 _specularColor;
        private float _specularPower;

        private bool _ambientLight;

        #region Constructors
        /// <summary>
        /// Constructor starting the light with diffuse color, ambient color, and direction
        /// </summary>
        /// <param name="diffuseColor">the diffuse color</param>
        /// <param name="ambientColor">the ambient color</param>
        /// <param name="direction">the direction</param>
        public BasicLight(string id, Vector3 position, Color4 diffuseColor, Color4 ambientColor, Vector3 direction) : base(id, position)
        {
            _diffuseColor = diffuseColor;
            _ambientColor = ambientColor;
            _direction = direction;

            _ambientLight = true;
        }

        /// <summary>
        /// Constructor for light with no ambient
        /// </summary>
        /// <param name="diffuseColor">diffuse color</param>
        /// <param name="direction">direction</param>
        public BasicLight(string id, Vector3 position, Color4 diffuseColor, Vector3 direction) : base(id, position)
        {
            _diffuseColor = diffuseColor;
            _direction = direction;

            _ambientLight = false;
            _ambientColor = new Color4(0.0f, 0.0f, 0.0f, 1.0f);
        }

        /// <summary>
        /// Constructor for light with specular light
        /// </summary>
        /// <param name="diffuseColor">diffuse color</param>
        /// <param name="ambientColor">ambient color</param>
        /// <param name="direction">direction</param>
        /// <param name="specularColor">specular color</param>
        /// <param name="specularPower">specular power</param>
        public BasicLight(string id, Vector3 position, Color4 diffuseColor, Color4 ambientColor, Vector3 direction, Color4 specularColor, float specularPower) : base(id, position)
        {
            _diffuseColor = diffuseColor;
            _ambientColor = ambientColor;
            _direction = direction;
            _specularColor = specularColor;
            _specularPower = specularPower;

            _ambientLight = true;
        }
        #endregion

        #region Setters
        /// <summary>
        /// Set the Diffuse Color for the light
        /// </summary>
        /// <param name="red">red component</param>
        /// <param name="green">green component</param>
        /// <param name="blue">blue component</param>
        /// <param name="alpha">alpha component</param>
        public void SetDiffuseColor(float red, float green, float blue, float alpha)
        {
            _diffuseColor = new Color4(red, green, blue, alpha);
        }

        /// <summary>
        /// Set the light's direction
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <param name="z">z value</param>
        public void SetDirection(float x, float y, float z)
        {
            _direction = new Vector3(x, y, z);
        }

        /// <summary>
        /// Set the ambient color of the light
        /// </summary>
        /// <param name="red">set the red component</param>
        /// <param name="green">set the green component</param>
        /// <param name="blue">set the blue component</param>
        /// <param name="alpha">set the alpha component</param>
        public void SetAmbientColor(float red, float green, float blue, float alpha)
        {
            _ambientColor = new Color4(red, green, blue, alpha);
        }

        /// <summary>
        /// Set the specular color of the light
        /// </summary>
        /// <param name="specularColor">the color to set</param>
        public void SetSpecularColor(Color4 specularColor)
        {
            _specularColor = specularColor;
        }

        /// <summary>
        /// Set the specular power of the light
        /// </summary>
        /// <param name="power">the power to set</param>
        public void SetSpecularPower(float power)
        {
            _specularPower = power;
        }

        /// <summary>
        /// Set the light as using ambient or not
        /// </summary>
        /// <param name="ambientLight">whether the light is now ambient or not</param>
        public void SetAmbientLight(bool ambientLight)
        {
            _ambientLight = ambientLight;
        }
        #endregion

        #region Getters
        public Color4 GetDiffuseColor()
        {
            return _diffuseColor;
        }

        public Vector3 GetDirection()
        {
            return _direction;
        }

        public Color4 GetAmbientColor()
        {
            return _ambientColor;
        }

        public Color4 GetSpecularColor()
        {
            return _specularColor;
        }

        public float GetSpecularPower()
        {
            return _specularPower;
        }
        #endregion
    }
}
