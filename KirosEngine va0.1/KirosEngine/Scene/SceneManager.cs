using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using KirosEngine.Exception;

namespace KirosEngine.Scene
{
    /// <summary>
    /// Manages all the scenes used in the game, is a singleton
    /// </summary>
    public class SceneManager
    {
        public static XNamespace SceneIndexNS = "http://kirosindustries.com/SceneIndex.xsd";
        private static SceneManager instance;
        private List<Scene> _scenes;

        private Device _device;
        private string _modelPath;
        private string _scenePath;

        private SceneManager()
        {
            _scenes = new List<Scene>();
        }
        
        /// <summary>
        /// Public accessor for the Scene Manager's instance
        /// </summary>
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SceneManager();
                }
                return instance;
            }
        }

        public string ScenePath
        {
            get
            {
                return _scenePath;
            }
        }

        /// <summary>
        /// Set the path currently in use for the scene data
        /// </summary>
        /// <param name="path">The path to set</param>
        public void SetScenePath(string path)
        {
            _scenePath = path;
        }

        public void SetModelPath(string path)
        {
            _modelPath = path;
        }

        public void SetDevice(Device device)
        {
            _device = device;
        }

        //returns true if the given scene id is in use
        public bool IdInUse(string id)
        {
            bool result = false;

            foreach (Scene s in _scenes)
            {
                if (s.SceneID.Equals(id))
                {
                    result = true;
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new scene to the manager using the provided xml data
        /// </summary>
        /// <param name="xml">The xml data to use</param>
        /// <exception cref="IDInUseException">Thrown when a given id is already in use</exception>
        /// <exception cref="InvalidDataValueException">Thrown when the data value read in is not of the expected type</exception>
        public void AddScene(XElement xml)
        {
            string sceneID = xml.Element(SceneIndexNS + "id").Value;
            if (this.IdInUse(sceneID))
            {
                throw new IDInUseException("The ID is already in use", sceneID, "Scene", this);
            }

            int sceneIndex = 0;
            if (!Int32.TryParse(xml.Attribute("index").Value, out sceneIndex))
            {
                //throw invalid value exception
                throw new InvalidDataValueException("Invalid data value recived", "int", "string", xml.Attribute("index").Value);
            }
            string name = xml.Element(SceneIndexNS + "name").Value;
            
            Scene scene = new Scene(sceneID, name, sceneIndex);
            _scenes.Add(scene);
        }
        
        /// <summary>
        /// Add a new scene to the manager
        /// </summary>
        /// <param name="scene">The scene to be added</param>
        public void AddScene(Scene scene)
        {
            if(this.IdInUse(scene.SceneID))
            {
        	    throw new IDInUseException("The ID is already in use", scene.SceneID, scene, this);
        	}
        	else
        	{
        	    _scenes.Add(scene);
        	}
        }

        public Scene LoadScene(int index)
        {
            bool result = true;
            Scene toLoad = _scenes.Find(x => x.SceneIndex == index);
            result = toLoad.Load(_device, _modelPath);

            return toLoad;
        }

        public bool UnloadScene(int index)
        {
            bool result = true;

            return result;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
