using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.DirectInput;
using SlimDX.RawInput;

using InputDevice = SlimDX.RawInput.Device;
using DeviceFlags = SlimDX.RawInput.DeviceFlags;
using Keys = System.Windows.Forms.Keys;

namespace KirosEngine.Input
{
    class InputCore
    {
        private Dictionary<Keys, KeyState> _currentKeyState;

        private int _screenHeight, _screenWidth;
        private int _mouseX, _mouseY;

        public void Initialize(int screenWidth, int screenHeight)
        {
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;

            InputDevice.RegisterDevice(SlimDX.Multimedia.UsagePage.Generic, SlimDX.Multimedia.UsageId.Keyboard, DeviceFlags.None);
            InputDevice.KeyboardInput += new EventHandler<KeyboardInputEventArgs>(KeyboardInput);

            InputDevice.RegisterDevice(SlimDX.Multimedia.UsagePage.Generic, SlimDX.Multimedia.UsageId.Mouse, DeviceFlags.None);
            InputDevice.MouseInput += new EventHandler<MouseInputEventArgs>(MouseInput);

            _currentKeyState = new Dictionary<Keys, KeyState>();

            _mouseX = 0;
            _mouseY = 0;
        }

        /// <summary>
        /// Receives keyboard input events and stores the key state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyboardInput(object sender, KeyboardInputEventArgs e)
        {
            if (!_currentKeyState.ContainsKey(e.Key))
            {
                _currentKeyState.Add(e.Key, e.State);
            }
            else
            {
                _currentKeyState[e.Key] = e.State;
            }
        }

        /// <summary>
        /// Receives mouse input events and stores the new position
        /// </summary>
        /// <param name="sender">The event's sender</param>
        /// <param name="e">The mouse event object</param>
        void MouseInput(object sender, MouseInputEventArgs e)
        {
            //needs work
            _mouseX += e.X;
            _mouseY += e.Y;

            if (_mouseX < 0) { _mouseX = 0; }
            if (_mouseY < 0) { _mouseY = 0; }

            if (_mouseX > _screenWidth) { _mouseX = _screenWidth; }
            if (_mouseY > _screenHeight) { _mouseY = _screenHeight; }
        }

        /// <summary>
        /// Public accessor for the current mouse location
        /// </summary>
        public Vector2 MouseLocation
        {
            get
            {
                return new Vector2(_mouseX, _mouseY);
            }
        }

        /// <summary>
        /// Check the key states to see if the given key is pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the key is pressed, false otherwise</returns>
        public bool KeyPressed(Keys key)
        {
            if (_currentKeyState.ContainsKey(key))
            {
                if (_currentKeyState[key] == KeyState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {

        }
    }
}
