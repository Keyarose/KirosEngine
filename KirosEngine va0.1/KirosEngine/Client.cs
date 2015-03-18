using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using SlimDX.DirectSound;
using SlimDX.DirectInput;
using KirosEngine.Input;
using KirosEngine.Scene;
using KirosEngine.Material;
using KirosEngine.Exception;
using KirosEngine.Textures;
using KirosEngine.Sound;
using KirosEngine.Camera;
using KirosEngine.Shader;
using KirosEngine.ScreenText;
using KirosEngine.EngineConsole;
using KirosEngine.Model;
using KirosEngine.Light;

using Device = SlimDX.Direct3D11.Device;
using Keys = System.Windows.Forms.Keys;
using Console = System.Console;

namespace KirosEngine
{
    /// <summary>
    /// The Player's side of the program
    /// </summary>
    class Client
    {
        D3DCore _core;
        Device _device;
        DeviceContext _context;
        InputCore _inputCore;
        DeviceCollection _soundDevices;
        DeviceCollection _soundInputDevices;

        protected RenderForm _clientForm;
        protected string _languageLocal = "en"; //defaults to en(English)
        protected string _defaultGamesDirectory = "games";
        protected bool _libraryMode;
        protected string _gameTitle;
        protected SceneManager _sceneManager;
        protected MaterialManager _materialManager;
        protected KeyboardHandler _keyHandler;
        protected Font _defaultFont;
        protected BaseCamera _defaultCamera;
        protected ClientConsole _clientConsole;
        protected Scene.Scene _loadedScene;

        static float rotation = 0.0f;

        //paths
        protected string _gamePath;
        protected string _scenePath;
        protected string _scriptPath;
        protected string _shaderPath;
        protected string _modelPath;
        protected string _texturePath;

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="title">Title of the Game being run by the client</param>
        public Client(string title)
        {
            _clientForm = new RenderForm(title);
            _core = new D3DCore();
            _inputCore = new InputCore();
        }

        /// <summary>
        /// Pre-game initialization
        /// </summary>
        /// <returns>Returns true if the initialization is successful</returns>
        public bool Initialize()
        {
            bool result = true;
            
            //logger init
            ErrorLogger.Initialize("log");

            //graphics init
            _core.Initialize(_clientForm, false, 0.1f, 1000.0f);
            _device = _core.GetDevice();
            _context = _device.ImmediateContext;

            ShaderManager.Instance.Init(_device, _context);
            MaterialManager.Instance.Init();
            //input init
            _inputCore.Initialize(_clientForm.Width, _clientForm.Height);
            _keyHandler = new KeyboardHandler(_inputCore);

            //sound init
            _soundDevices = DirectSound.GetDevices();
            _soundInputDevices = DirectSoundCapture.GetDevices();

            //init the default text shader and font
            ShaderManager.Instance.AddShader(@"resources/Shaders/DX/font.vs", @"resources/shaders/DX/font.ps", "FontVertexShader", "FontPixelShader", 
                Text.DefaultTextShaderKey, ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer | ShaderBufferFlags.PixelBuffer, new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0), 
                                new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0) });
            _defaultFont = new Font();
            _defaultFont.Initialize(_device, @"resources/fonts/arial16base.fnt", @"resources/fonts/arial16base_0.png", 16);

            //init the default camera
            _defaultCamera = new BaseCamera();
            _defaultCamera.SetPosition(new Vector3(0.0f, 0.0f, -10.0f));
            _defaultCamera.Draw();

            //init the console
            _clientConsole = new ClientConsole(_device, _defaultFont, _clientForm.Height, _clientForm.Width, _keyHandler);
            return result;
        }

        //TODO: Complete scrap of library mode
        #region Library Mode
        /// <summary>
        /// Begin loading the client in library mode, rather than a specific game
        /// </summary>
        /// <returns>Returns true if it is able to load successfully</returns>
        public bool Load()
        {
            _libraryMode = true;
            bool result = true;
            //check the default path for game data folders and get a list of them
            string[] gameFolders = Directory.GetDirectories(_defaultGamesDirectory);
            GameData[] gameData = new GameData[gameFolders.Length];
            int count = 0;
            
            foreach(string gameFolder in gameFolders)
            {
                string gameDataFilePath = gameFolder + "/data.xml";
                gameData[count] = this.LoadData(XDocument.Load(gameDataFilePath));
                
                count++;
            }
            return result;
        }
        
        /// <summary>
        /// Load the data for the given game document
        /// </summary>
        /// <param name="xml">The XDocument to load the data from</param>
        /// <returns>Returns a GameData object containing the information</returns>
        protected GameData LoadData(XDocument xml)
        {
            GameData data = new GameData();
            //load the title
            XElement titles = xml.Root.Element("titles");
            var title = from langTitle in titles.Elements("title")
                        where (string)langTitle.Attribute("lang") == _languageLocal
                        select langTitle;
            foreach(XElement t in title)
            {
                data.Title = t.Value;
            }
            data.IconPath = xml.Root.Element("icon").Value;
            data.Copyright = xml.Root.Element("copyright").Value;
            
            return data;
        }
        #endregion

        /// <summary>
        /// Begin loading the first scene and the game data(probably the main menu or opening animation/credits)
        /// </summary>
        /// <param name="filePath">The file path to the game data</param>
        /// <exception cref="FileNotFoundException">Thrown when the provided file cannot be found</exception>
        /// <returns>Returns true if it is able to load successfully</returns>
        public bool Load(string filePath)
        {
            _libraryMode = false;
            bool result = true;
            
            _gamePath = filePath;
            ErrorLogger.Instance.FilePath = _gamePath;
            //the default path for the data file is just inside the game folder
            string gameDataFilePath = filePath + "/data.xml";
            if (File.Exists(gameDataFilePath))
            {
                XDocument gameData = XDocument.Load(gameDataFilePath);
                this.ProcessGameData(gameData);
            }
            else
            {
                result = false;
                ErrorLogger.Write(String.Format("The main data file was not found at: {0}", gameDataFilePath));
                throw new FileNotFoundException(String.Format("The main data file was not found at: {0}", gameDataFilePath), gameDataFilePath);
            }
            
            //open and read the scene index, get the file names of each scene file
            string sceneIndexFilePath = _gamePath + "/" + _scenePath + "/index.xml";
            if(File.Exists(sceneIndexFilePath))
            {
            	XDocument sceneIndex = XDocument.Load(sceneIndexFilePath);
            	this.LoadSceneIndexData(sceneIndex);
            }
            else
            {
            	result = false;
                ErrorLogger.Write(String.Format("The scene index file was not found at: {0}", sceneIndexFilePath));
            	throw new FileNotFoundException(String.Format("The scene index file was not found at: {0}", sceneIndexFilePath), sceneIndexFilePath);
            }
            _loadedScene = SceneManager.Instance.LoadScene(0);
            
            //Exception Debuging
            //foreach (Type t in ExceptionAnalyser.GetAllExceptions(typeof(BaseSound).GetMethod("LoadWaveFile")))
            //{
            //    Console.WriteLine(t);
            //}
            
            return result;
        }

        /// <summary>
        /// Load the information from the data file
        /// </summary>
        /// <param name="xml">The xml file to load from</param>
        protected void ProcessGameData(XDocument xml)
        {
            //Game title loading
            XNamespace gameDataNS = "http://kirosindustries.com/GameData.xsd";
            XElement titles = xml.Root.Element(gameDataNS + "titles");
            var title = from langTitle in titles.Elements(gameDataNS + "title")
                        where (string)langTitle.Attribute("lang") == _languageLocal
                        select langTitle;

            foreach (XElement t in title)
            {
                _gameTitle = t.Value;
            }
            //end game title loading

            //get the data paths
            XElement paths = xml.Root.Element(gameDataNS + "paths");
            try
            {
                _scriptPath = paths.Element(gameDataNS + "scriptPath").Value;
                _scenePath = paths.Element(gameDataNS + "scenePath").Value;
                _modelPath = paths.Element(gameDataNS + "modelPath").Value;
                _texturePath = paths.Element(gameDataNS + "texturePath").Value;
                _shaderPath = paths.Element(gameDataNS + "shaderPath").Value;
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("One or more paths are missing from the data file, unable to continue.");
            }
            //end getting the data paths

            //update the form with the game title
            _clientForm.Text = _gameTitle;
            TextureManager.Instance.SetTexturePath(_gamePath + "/" + _texturePath);
            ShaderManager.Instance.SetShaderPath(_gamePath + "/" + _shaderPath);
        }
        
        protected void LoadSceneIndexData(XDocument xml)
        {
        	//init the scene manager
            _sceneManager = SceneManager.Instance;
            _sceneManager.SetScenePath(_gamePath + "/" + _scenePath);
            _sceneManager.SetModelPath(_gamePath + "/" + _modelPath);
            _sceneManager.SetDevice(_device);

            //get all the scene elements in the index
            XNamespace sceneNS = "http://kirosindustries.com/SceneIndex.xsd";
            XElement scenes = xml.Root;
            var scene = from sc in scenes.Elements(sceneNS + "scene")
                        select sc;

            //add each scene to the manager
            foreach (XElement s in scene)
            {
                try
                {
                    _sceneManager.AddScene(s);
                }
                catch(KirosException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void Run()
        {
            //TODO: handle resize events
            _clientForm.Resize += (o, e) =>
                {

                };

            MessagePump.Run(_clientForm, () =>
                {
                    //global timer tick here?
                    //clear background first
                    this.Update();
                    this.Draw();
                });

            //TODO: handle the window being closed by the used clicking the x
            _clientForm.FormClosing += (o, e) =>
                {

                };
        }

        /// <summary>
        /// Save the game State
        /// </summary>
        public void Save()
        {

        }

        /// <summary>
        /// Unload the game Content
        /// </summary>
        public void Unload()
        {
            _core.Dispose();
            _clientConsole.Dispose();
        }

        /// <summary>
        /// Update the world
        /// </summary>
        public void Update()
        {
            _keyHandler.Update();
            _clientConsole.Update();
        }

        /// <summary>
        /// Draw the view
        /// </summary>
        public void Draw()
        {
            _core.BeginScene(new Color4(0.5f, 0.5f, 1.0f));

            Matrix viewMatrix, worldMatrix, projectionMatrix, orthoMatrix;

            _defaultCamera.Draw();
            viewMatrix = _defaultCamera.GetViewMatrix();
            worldMatrix = _core.GetWorldMatrix();
            projectionMatrix = _core.GetProjectionMatrix();
            orthoMatrix = _core.GetOrthoMatrix();

            rotation += (float)(Math.PI * 0.0001f);

            Matrix worldMatrixR = Matrix.Multiply(worldMatrix, Matrix.RotationY(rotation));

            //_testModel.Draw(_context);
            //_lightShader.Draw(_context, _testModel.GetIndexCount(), worldMatrixR, projectionMatrix, viewMatrix, _testModel.GetTexture(), _testLight, _defaultCamera.Position);
            //draw the loaded scene
            _loadedScene.Draw(_context, viewMatrix, worldMatrixR, projectionMatrix);

            _core.TurnZBufferOff();
            _core.TurnOnAlphaBlending();
            //do stuff in 2d
            _clientConsole.Draw(_context, worldMatrix, viewMatrix, orthoMatrix);

            _core.TurnOffAlphaBlending();
            _core.TurnZBufferOn();

            _core.EndScene();
        }
    }
}
