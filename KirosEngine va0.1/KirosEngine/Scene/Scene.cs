using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using KirosEngine.Camera;
using KirosEngine.Light;
using KirosEngine.Model;
using KirosEngine.Shader;
using KirosEngine.Textures;
using KirosEngine.Material;
using KirosEngine.Exception;

using Device = SlimDX.Direct3D11.Device;

namespace KirosEngine.Scene
{
    public class Scene
    {
        //TODO: scene documentation
        public static XNamespace SceneNS = "http://kirosindustries.com/Scene.xsd";

        private Device _device;
        private string _modelPath;

        private string _sceneID;
        private string _sceneName;
        private int _sceneIndex;
        //change List type, rename?
        private List<SceneNode> _nodes;
        private List<BasicLight> _lights;
        private List<BaseCamera> _cameras;
        private BaseCamera _defaultCamera;
        private List<object> _items;
        private List<object> _soundEmitters;
        private List<object> _particleEmitters;

        /// <summary>
        /// Public accessor for the scene's id
        /// </summary>
        public string SceneID
        {
            get
            {
                return _sceneID;
            }
        }
        
        /// <summary>
        /// Public accessor for the scene's name
        /// </summary>
        public string SceneName
        {
            get
            {
                return _sceneName;
            }
        }

        /// <summary>
        /// Public accessor for the scene's index
        /// </summary>
        public int SceneIndex
        {
            get
            {
                return _sceneIndex;
            }
        }

        //default constructor
        public Scene()
        {
            //default scene id is 000000
            _sceneID = "000000";
            if (SceneIdInUse(_sceneID))
            {
                //id in use find another
            }
        }
        
        public Scene(string id, string name, int index)
        {
            _sceneID = id;
            _sceneName = name;
            _sceneIndex = index;

            _lights = new List<BasicLight>();
            _nodes = new List<SceneNode>();
        }

        //load the scene from the given XML file name
        public Scene(string xmlFile)
        {
            _lights = new List<BasicLight>();
            _nodes = new List<SceneNode>();

            XDocument xmlDoc = XDocument.Load(xmlFile);
            ProcessXml(xmlDoc);
        }

        //load the scene from the given XML document
        public Scene(XDocument xml)
        {
            _lights = new List<BasicLight>();
            _nodes = new List<SceneNode>();

            ProcessXml(xml);
        }

        public bool Load(Device device, string modelPath)
        {
            bool result = true;
            _device = device;
            _modelPath = modelPath;
            string fileName = SceneID.Split('_').Last();

            string scenePath = SceneManager.Instance.ScenePath;

            this.ProcessXml(XDocument.Load(scenePath + "/" + fileName + ".xml"));

            _defaultCamera = new BaseCamera();
            _defaultCamera.SetPosition(new Vector3(0.0f, 0.0f, -10.0f));
            _defaultCamera.Draw();

            return result;
        }

        //process the XML and load the data into the proper variables
        private bool ProcessXml(XDocument xml)
        {
            XElement scene = xml.Root;
            string sceneID = scene.Element(SceneNS + "id").Value;
            if(!_sceneID.Equals(sceneID))
            {
                ErrorLogger.Write(string.Format("The scene being loaded does not have an ID that matches that of the scene file being loaded\n" 
                    + "\tScene ID: {0} Scene File ID: {1}", _sceneID, sceneID));
                return false;
            }
            var shaders = from sh in scene.Element(SceneNS + "resources").Element(SceneNS + "shaders").Elements(SceneNS + "shader")
                          select sh;
            foreach(XElement shader in shaders)
            {
                this.LoadShader(shader);
            }

            var textures = from tx in scene.Element(SceneNS + "resources").Element(SceneNS + "textures").Elements(SceneNS + "texture")
                           select tx;
            foreach(XElement texture in textures)
            {
                this.LoadTexture(texture);
            }

            var materials = from mat in scene.Element(SceneNS + "resources").Element(SceneNS + "materials").Elements(SceneNS + "material")
                            select mat;
            foreach(XElement material in materials)
            {
                this.LoadMaterial(material);
            }

            var lights = from li in scene.Element(SceneNS + "objects").Element(SceneNS + "lights").Elements(SceneNS + "light")
                         select li;
            foreach(XElement light in lights)
            {
                this.LoadLight(light);
            }

            var models = from mo in scene.Element(SceneNS + "objects").Element(SceneNS + "models").Elements(SceneNS + "model")
                         select mo;
            foreach(XElement model in models)
            {
                this.LoadModel(model);
            }

            return true;
        }

        private void LoadModel(XElement model)
        {
            string modelFile = model.Element(SceneNS + "fileName").Value;
            string textureID = model.Element(SceneNS + "texture").Value;
            string materialID = model.Element(SceneNS + "material").Value;
            XElement position = model.Element(SceneNS + "position");

            FileModel newModel = new FileModel(_device, _modelPath +"/"+ modelFile, MaterialManager.Instance.GetMaterialForKey(materialID));
            newModel.SetPosition(new Vector3(float.Parse(position.Attribute("x").Value), float.Parse(position.Attribute("y").Value), float.Parse(position.Attribute("z").Value)));
            _nodes.Add(newModel);
        }

        private void LoadLight(XElement light)
        {
            XElement diffuseColor = light.Element(SceneNS + "diffuseColor");
            XElement ambientColor = light.Element(SceneNS + "ambientColor");
            XElement specularColor = light.Element(SceneNS + "specularColor");
            XElement direction = light.Element(SceneNS + "direction");

            Color4 diffuse = new Color4(float.Parse(diffuseColor.Attribute("r").Value), float.Parse(diffuseColor.Attribute("g").Value), float.Parse(diffuseColor.Attribute("b").Value), float.Parse(diffuseColor.Attribute("a").Value));
            Color4 ambient = new Color4(float.Parse(diffuseColor.Attribute("r").Value), float.Parse(diffuseColor.Attribute("g").Value), float.Parse(diffuseColor.Attribute("b").Value), float.Parse(diffuseColor.Attribute("a").Value));
            Color4 specular = new Color4(float.Parse(diffuseColor.Attribute("r").Value), float.Parse(diffuseColor.Attribute("g").Value), float.Parse(diffuseColor.Attribute("b").Value), float.Parse(diffuseColor.Attribute("a").Value));
            Vector3 direct = new Vector3(float.Parse(direction.Attribute("x").Value), float.Parse(direction.Attribute("y").Value), float.Parse(direction.Attribute("z").Value));

            BasicLight newLight = new BasicLight("", new Vector3(), diffuse, ambient, direct, specular, float.Parse(light.Element(SceneNS + "specularPower").Value));
            _lights.Add(newLight);
        }

        private void LoadTexture(XElement texture)
        {
            string textureID = texture.Element(SceneNS + "id").Value;
            string file = TextureManager.Instance.TexturePath + "/" + texture.Element(SceneNS + "fileName").Value;

            Texture t1 = new Texture(file, textureID);
            TextureManager.Instance.AddTexture(t1);
            t1.Initialize(_device);
        }

        private void LoadMaterial(XElement xml)
        {
            string materialName = xml.Element(SceneNS + "name").Value;
            string shaderID = xml.Element(SceneNS + "shader").Value;
            string textureID = xml.Element(SceneNS + "texture").Value;

            ObjectMaterial mat = new ObjectMaterial(materialName, textureID, shaderID);
            bool result = MaterialManager.Instance.AddMaterial(materialName, mat);

            if(!result)
            {
                ErrorLogger.Write(string.Format("Failed to add material to the material manager for id: {0}", materialName));
            }
        }

        private void LoadShader(XElement xml)
        {
            string shaderID = xml.Element(SceneNS + "id").Value;
            string vertexFile = ShaderManager.Instance.ShaderPath + "/" + xml.Element(SceneNS + "vertFile").Value;
            string vertexMethod = xml.Element(SceneNS + "vertMethod").Value;
            string pixelFile = ShaderManager.Instance.ShaderPath + "/" + xml.Element(SceneNS + "pixelFile").Value;
            string pixelMethod = xml.Element(SceneNS + "pixelMethod").Value;

            //get the shader's data elements
            List<XElement> shaderElements = xml.Element(SceneNS + "shaderElements").Elements(SceneNS + "shaderElement").ToList();

            InputElement[] shaderInputElements = new InputElement[shaderElements.Count];
            for (int i = 0; i < shaderElements.Count; i++ )
            {
                string name = shaderElements[i].Element(SceneNS + "name").Value;
                int index = int.Parse(shaderElements[i].Element(SceneNS + "index").Value);
                Format format = BaseShader.FormatDictionary[shaderElements[i].Element(SceneNS + "format").Value];
                int offset = BaseShader.OffsetDictionary[shaderElements[i].Element(SceneNS + "offset").Value];
                int slot = int.Parse(shaderElements[i].Element(SceneNS + "slot").Value);
                InputClassification inputClass = BaseShader.InputDictionary[shaderElements[i].Element(SceneNS + "inputClass").Value];
                int stepRate = int.Parse(shaderElements[i].Element(SceneNS + "stepRate").Value);

                shaderInputElements[i] = new InputElement(name, index, format, offset, slot, inputClass, stepRate);
            }

            ShaderBufferFlags bufferFlags = ShaderBufferFlags.None;
            XElement shaderConstants = xml.Element(SceneNS + "shaderConstants");

            if (bool.Parse(shaderConstants.TryGetElementValue("lightConstant", SceneNS, "false")))
            {
                bufferFlags = bufferFlags | ShaderBufferFlags.LightBuffer;
            }
            if (bool.Parse(shaderConstants.TryGetElementValue("cameraConstant", SceneNS, "false")))
            {
                bufferFlags = bufferFlags | ShaderBufferFlags.CameraBuffer;
            }
            if (bool.Parse(shaderConstants.TryGetElementValue("pixelConstant", SceneNS, "false")))
            {
                bufferFlags = bufferFlags | ShaderBufferFlags.PixelBuffer;
            }
            if (bool.Parse(shaderConstants.Element(SceneNS + "matrixConstant").Value))
            {
                bufferFlags = bufferFlags | ShaderBufferFlags.MatrixBuffer;
            }
            if (bool.Parse(shaderConstants.TryGetElementValue("samplerConstant", SceneNS, "false")))
            {
                bufferFlags = bufferFlags | ShaderBufferFlags.SamplerBuffer;
            }

            BaseShader temp = null;
            if (ShaderManager.Instance.AddShader(vertexFile, pixelFile, vertexMethod, pixelMethod, shaderID, bufferFlags, shaderInputElements))
            {
                temp = ShaderManager.Instance.GetShaderForKey(shaderID);
            }

            //TODO: finish shader constant buffers
            if(temp != null)
            {
                SamplerDescription sampleDisc = new SamplerDescription();

                if (bool.Parse(shaderConstants.TryGetElementValue("samplerConstant", SceneNS, "false")))
                {
                    XElement samplerDiscript = xml.Element(SceneNS + "samplerDiscription");
                    XElement bColor = samplerDiscript.Element(SceneNS + "borderColor");

                    sampleDisc = new SamplerDescription
                    {
                        Filter = BaseShader.FilterDictionary[samplerDiscript.Element(SceneNS + "filter").Value],
                        AddressU = BaseShader.TextureAddressDictionary[samplerDiscript.Element(SceneNS + "addressUMode").Value],
                        AddressV = BaseShader.TextureAddressDictionary[samplerDiscript.Element(SceneNS + "addressVMode").Value],
                        AddressW = BaseShader.TextureAddressDictionary[samplerDiscript.Element(SceneNS + "addressWMode").Value],
                        MipLodBias = float.Parse(samplerDiscript.Element(SceneNS + "lodBias").Value),
                        MaximumAnisotropy = int.Parse(samplerDiscript.TryGetElementValue("maxAnisotropy", SceneNS, "1")),
                        ComparisonFunction = BaseShader.ComparisonDictionary[samplerDiscript.TryGetElementValue("comparisonFunction", SceneNS, "Always")],
                        BorderColor = new Color4(float.Parse(bColor.Attribute("r").Value), float.Parse(bColor.Attribute("g").Value), float.Parse(bColor.Attribute("b").Value), float.Parse(bColor.Attribute("a").Value)),
                        MinimumLod = float.Parse(samplerDiscript.TryGetElementValue("minLod", SceneNS, "0.0")),
                        MaximumLod = float.Parse(samplerDiscript.TryGetElementValue("maxLod", SceneNS, float.MaxValue.ToString()))
                    };

                    temp.SetSamplerDiscription(sampleDisc);
                }

                XElement[] bufferDescriptions = xml.Element(SceneNS + "bufferDescriptions").Elements(SceneNS + "buffer").ToArray();

                //get each of the buffers for the shader
                foreach(XElement buffer in bufferDescriptions)
                {
                    string id = buffer.Attribute("name").Value;
                    BufferDescription descrip = new BufferDescription
                    {
                        Usage = BaseShader.ResourceUsageDictionary[buffer.TryGetElementValue("resourceUsage", SceneNS, "Dynamic")],
                        SizeInBytes = int.Parse(buffer.TryGetElementValue("sizeInBytes", SceneNS, "0")),
                        BindFlags = BaseShader.BindFlagsDictionary[buffer.TryGetElementValue("bindFlags", SceneNS, "ConstantBuffer")],
                        CpuAccessFlags = BaseShader.CpuAccessFlagsDictionary[buffer.TryGetElementValue("cpuAccessFlags", SceneNS, "Write")],
                        OptionFlags = BaseShader.ResourceOptionFlagsDictionary[buffer.TryGetElementValue("optionFlags", SceneNS, "None")],
                        StructureByteStride = int.Parse(buffer.TryGetElementValue("byteStride", SceneNS, "0")) 
                    };

                    temp.AddBufferDescription(id, descrip);
                }
            }
        }

        //check the given id to see if it is in use
        private bool SceneIdInUse(string id)
        {
            return SceneManager.Instance.IdInUse(id);
        }

        public void Draw(DeviceContext context, Matrix viewMatrix, Matrix worldMatrix, Matrix projectionMatrix)
        {
            //BaseShader shader = ShaderManager.Instance.GetShaderForKey("simpleLight");
            foreach(FileModel model in _nodes)
            {
                model.Draw(context, worldMatrix, projectionMatrix, viewMatrix, _defaultCamera, _lights.ToArray());
                //if(shader.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.LightBuffer | ShaderBufferFlags.CameraBuffer | ShaderBufferFlags.SamplerBuffer))
                //{
                //    shader.Draw(context, model.IndexCount, Matrix.Translation(model.Position) * worldMatrix, projectionMatrix, viewMatrix, model.GetTexture(), _lights[0], _defaultCamera.Position);
                //}
                //else if(shader.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer | ShaderBufferFlags.PixelBuffer))
                //{
                //    shader.Draw(context, model.IndexCount, Matrix.Translation(model.Position) * worldMatrix, projectionMatrix, viewMatrix, model.GetTexture(), new Vector4());
                //}
                //else if(shader.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.LightBuffer | ShaderBufferFlags.CameraBuffer))
                //{
                //    shader.Draw(context, model.IndexCount, Matrix.Translation(model.Position) * worldMatrix, projectionMatrix, viewMatrix, _lights[0], _defaultCamera.Position);
                //}
                //else if(shader.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer))
                //{
                //    shader.Draw(context, model.IndexCount, Matrix.Translation(model.Position) * worldMatrix, projectionMatrix, viewMatrix, model.GetTexture());
                //}
                //else if(shader.ShaderBufferFlags.HasFlag(ShaderBufferFlags.MatrixBuffer))
                //{
                //    shader.Draw(context, model.IndexCount, Matrix.Translation(model.Position) * worldMatrix, projectionMatrix, viewMatrix);
                //}
            }
        }
    }
}