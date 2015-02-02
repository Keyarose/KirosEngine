using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using KirosEngine.ScreenText;
using KirosEngine.Input;
using KirosEngine.Shader;

using Device = SlimDX.Direct3D11.Device;
using KeyEventHandler = KirosEngine.Input.KeyEventHandler;
using KeyEventArgs = KirosEngine.Input.KeyEventArgs;

namespace KirosEngine.EngineConsole
{
    class ClientConsole
    {
        public const string ToggleAction = "toggleConsole";

        private CpuMonitor _cpuMonitor;
        private FpsCounter _fpsCounter;
        private RamMonitor _ramMonitor;

        private Text _fpsText;
        private Text _cpuText;
        private Text _ramText;

        private KeyboardHandler _keyHandler;

        private bool _visable = true;

        public bool Visable
        {
            get
            {
                return _visable;
            }
            set
            {
                _visable = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="font">The font used by the console</param>
        /// <param name="screenHeight">The screen height</param>
        /// <param name="screenWidth">The screen width</param>
        /// <param name="keyHandler">The key handler</param>
        public ClientConsole(Device device, Font font, int screenHeight, int screenWidth, KeyboardHandler keyHandler)
        {
            _cpuMonitor = new CpuMonitor();
            _fpsCounter = new FpsCounter();
            _ramMonitor = new RamMonitor();

            BaseShader textShader = ShaderManager.Instance.GetShaderForKey(Text.DefaultTextShaderKey);

            _fpsText = new Text(device, screenHeight, screenWidth, font, textShader)
            {
                Position = new Vector2(0, 0),
                Color = new Vector4(1.0f)
            };
            _cpuText = new Text(device, screenHeight, screenWidth, font, textShader)
            {
                Position = new Vector2(0, 20),
                Color = new Vector4(1.0f)
            };
            _ramText = new Text(device, screenHeight, screenWidth, font, textShader)
            {
                Position = new Vector2(0, 40),
                Color = new Vector4(1.0f)
            };

            _keyHandler = keyHandler;
            _keyHandler.AddBindings(Keys.Oemtilde, ToggleAction, KeyMode.Pressed);
            _keyHandler.KeyPressed += new KeyEventHandler(KeyPressed);
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            KeyboardBindings bindings = _keyHandler.KeyboardBindings;

            if(e.Key == bindings.GetKey(ToggleAction))
            {
                this._visable = !_visable;
            }
        }

        /// <summary>
        /// Update the state of the console
        /// </summary>
        public void Update()
        {
            _fpsCounter.Update();

            _fpsText.Verse = string.Format("FPS: {0}", _fpsCounter.GetFps());
            _cpuText.Verse = string.Format("Total CPU: {0}", _cpuMonitor.GetCpuUsage());
            _ramText.Verse = string.Format("RAM: {0}", _ramMonitor.GetAvailableRam());
        }

        /// <summary>
        /// Draw the console if it is visable
        /// </summary>
        /// <param name="context">The device context</param>
        /// <param name="worldMatrix">The world matrix</param>
        /// <param name="viewMatrix">The view matrix</param>
        /// <param name="orthoMatrix">The ortho matrix</param>
        public void Draw(DeviceContext context, Matrix worldMatrix, Matrix viewMatrix, Matrix orthoMatrix)
        {
            if(_visable)
            {
                _fpsText.Draw(context, worldMatrix, viewMatrix, orthoMatrix);
                _cpuText.Draw(context, worldMatrix, viewMatrix, orthoMatrix);
                _ramText.Draw(context, worldMatrix, viewMatrix, orthoMatrix);
            }
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            _cpuMonitor.Dispose();
            _ramMonitor.Dispose();

            _fpsText.Dispose();
            _cpuText.Dispose();
            _ramText.Dispose();
        }
    }
}
