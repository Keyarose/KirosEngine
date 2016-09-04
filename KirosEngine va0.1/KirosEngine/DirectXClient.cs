using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.DirectSound;
using SlimDX.Windows;
using KirosEngine.Input;
using KirosEngine.Shader;
using KirosEngine.Material;
using KirosEngine.ScreenText;
using KirosEngine.Camera;
using KirosEngine.EngineConsole;
using KirosEngine.Events;
using KirosEngine.Scene;

using Device = SlimDX.Direct3D11.Device;
using Keys = System.Windows.Forms.Keys;
using Console = System.Console;

namespace KirosEngine
{
    class DirectXClient
    {
        protected D3DCore _core;
        protected Device _device;
        protected DeviceContext _context;
        protected InputCore _inputCore;

        protected RenderForm _clientForm;
        protected KeyboardHandler _keyHandler;
        protected DeviceCollection _soundDevices;
        protected DeviceCollection _soundInputDevices;
        protected Font _defaultFont;
        protected BaseCamera _defaultCamera;
        protected ClientConsole _console;
        protected Scene.Scene _loadedScene;
        protected string _languageLocal = "en";
        protected string _gameTitle;

        //paths
        protected string _gamePath;
        protected string _scenePath;

        public DirectXClient(string title)
        {
            _clientForm = new RenderForm(title);
            _core = new D3DCore();
            _inputCore = new InputCore();
        }

        public bool Initialize()
        {
            bool result = true;

            //init logger
            ErrorLogger.Initialize("log");

            //init graphics
            if(!_core.Initialize(_clientForm, false, 0.1f, 1000.0f))
            {
                return false;
            }
            _device = _core.GetDevice();
            _context = _device.ImmediateContext;

            ShaderManager.Instance.Init(_device, _context);
            MaterialManager.Instance.Init();

            //init input
            _inputCore.Initialize(_clientForm.Width, _clientForm.Height);
            _keyHandler = new KeyboardHandler(_inputCore);

            //init sound
            _soundDevices = DirectSound.GetDevices();
            _soundInputDevices = DirectSoundCapture.GetDevices();

            //init default font and font shader
            ShaderManager.Instance.AddShader(@"resources/Shaders/DX/font.vs", @"resources/shaders/DX/font.ps", "FontVertexShader", "FontPixelShader", 
                Text.DefaultTextShaderKey, ShaderBufferFlags.MatrixBuffer | ShaderBufferFlags.SamplerBuffer | ShaderBufferFlags.PixelBuffer, new [] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0), 
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0) });
            _defaultFont = new Font();
            _defaultFont.Initialize(_device, @"resources/fonts/arial16base.fnt", @"resources/fonts/arial16base_0.png", 16);

            //init default camera
            _defaultCamera = new BaseCamera();
            _defaultCamera.SetPosition(new Vector3(0.0f, 0.0f, -10.0f));
            _defaultCamera.Draw();

            //init the console
            _console = new ClientConsole(_device, _defaultFont, _clientForm.Height, _clientForm.Width, _keyHandler);

            return result;
        }

        public virtual bool Load(string filePath)
        {
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
            string sceneIndexFilePath = _gamePath + "/" + _scenePath + "/" + "index.xml";
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

            return result;
        }

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
        }

        protected void LoadSceneIndexData(XDocument xml)
        {

        }
    }
}
