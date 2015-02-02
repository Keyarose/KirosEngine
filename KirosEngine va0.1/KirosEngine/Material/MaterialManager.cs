using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KirosEngine.Exception;

namespace KirosEngine.Material
{
    /// <summary>
    /// Manages and loads materials used by the models
    /// </summary>
    class MaterialManager
    {
        private Dictionary<string, ObjectMaterial> _materials;
        private static MaterialManager _instance;

        public static MaterialManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new MaterialManager();
                }
                return _instance;
            }
        }

        private MaterialManager()
        {
            _materials = new Dictionary<string, ObjectMaterial>();
        }

        public void Init()
        {

        }

        /// <summary>
        /// Add a material with the given id to the manager
        /// </summary>
        /// <param name="id">The id to register the material with</param>
        /// <param name="material">The material to register</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool AddMaterial(string id, ObjectMaterial material)
        {
            if(this.IdInUse(id))
            {
                throw new IDInUseException("The id is already in use", id, material, this);
            }

            _materials.Add(id, material);

            return true;
        }

        /// <summary>
        /// Get the material for the given id
        /// </summary>
        /// <param name="id">The id to get the material for</param>
        /// <returns>The object material for the id, or null</returns>
        public ObjectMaterial GetMaterialForKey(string key)
        {
            if(IdInUse(key))
            {
                return _materials[key];
            }

            return null;
        }

        /// <summary>
        /// Check to see if the given id is in use
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if the key is in use, false if it is not</returns>
        public bool IdInUse(string id)
        {
            return _materials.ContainsKey(id);
        }
    }
}
