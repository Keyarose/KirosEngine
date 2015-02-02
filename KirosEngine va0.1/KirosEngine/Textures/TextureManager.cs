using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using KirosEngine.Exception;

namespace KirosEngine.Textures
{
    class TextureManager
    {
        private static TextureManager _instance;
        private List<Texture> _textures;
        private string _texturePath;

        private TextureManager()
        {
            _textures = new List<Texture>();
        }

        /// <summary>
        /// Public accessor for the Texture Managaer
        /// </summary>
        public static TextureManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TextureManager();
                }
                return _instance;
            }
        }

        public string TexturePath
        {
            get
            {
                return _texturePath;
            }
        }

        /// <summary>
        /// Set the file path for the textures
        /// </summary>
        /// <param name="path"></param>
        public void SetTexturePath(string path)
        {
            _texturePath = path;
        }

        /// <summary>
        /// Checks to see if the given id is already used for a texture
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IdInUse(string id)
        {
            bool result = false;

            foreach (Texture t in _textures)
            {
                if (t.TextureID.Equals(id))
                {
                    result = true;
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Register a texture using data from xml
        /// </summary>
        /// <param name="xml"></param>
        public void AddTexture(XElement xml)
        {

        }

        /// <summary>
        /// Register the given texture with the manager
        /// </summary>
        /// <param name="texture">The texture to register</param>
        public void AddTexture(Texture texture)
        {
            if(this.IdInUse(texture.TextureID))
            {
                throw new IDInUseException("The ID is already in use", texture.TextureID, texture, this);
            }
            else
            {
                _textures.Add(texture);
            }
        }

        public Texture GetTexture(string id)
        {
            foreach(Texture t in _textures)
            {
                if(t.TextureID.Equals(id))
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove the texture for the given id
        /// </summary>
        /// <param name="id">The id of the texture to remove</param>
        public void RemoveTexture(string id)
        {
            foreach(Texture t in _textures)
            {
                if(t.TextureID.Equals(id))
                {
                    //remove it
                    t.Dispose();
                    _textures.Remove(t);
                    return;
                }
            }
            //tried to remove a texture that isnt registered
            ErrorLogger.Write(string.Format("Attempted to remove a texture that was not registered, Texture ID: {0}", id));
        }

        public void Dispose()
        {
            foreach(Texture t in _textures)
            {
                t.Dispose();
            }
        }
    }
}
