using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KirosEngine.Input
{
    /// <summary>
    /// Delegate method for key events
    /// </summary>
    /// <param name="sender">The instance of keyboardHandler sending the event call</param>
    /// <param name="e">The arguments for the key event</param>
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    class KeyboardHandler
    {
        private InputCore _inputCore;
        private KeyboardBindings _keyboardBindings;

        private Dictionary<Keys, bool> _previousKeyState;

        #region Events
        /// <summary>
        /// Key event handler for keys pressed
        /// </summary>
        public event KeyEventHandler KeyPressed;
        
        /// <summary>
        /// Key event handler for keys held
        /// </summary>
        public event KeyEventHandler KeyHeld;

        /// <summary>
        /// Key event handler for keys released
        /// </summary>
        public event KeyEventHandler KeyReleased;
        #endregion

        /// <summary>
        /// Public accessor for keyboard bindings
        /// </summary>
        public KeyboardBindings KeyboardBindings
        {
            get
            {
                return _keyboardBindings;
            }
        }

        /// <summary>
        /// Base constructor, initializes the collection classes
        /// </summary>
        /// <param name="core">The input core</param>
        public KeyboardHandler(InputCore core)
        {
            _inputCore = core;
            _keyboardBindings = new KeyboardBindings();
            _previousKeyState = new Dictionary<Keys, bool>();
        }

        /// <summary>
        /// Initialize the keyboard handler with the path to the key binding file
        /// </summary>
        /// <param name="bindingsFile">The file to read the bindings from</param>
        public void Init(string bindingsFile)
        {
            string fileType = bindingsFile.Split('.')[1];

            if (fileType.Equals("xml", StringComparison.CurrentCultureIgnoreCase))
            {
                _keyboardBindings.ReadBindingsFromFileXML(bindingsFile);
            }
        }

        /// <summary>
        /// Add a new key binding
        /// </summary>
        /// <param name="key">The key to bind</param>
        /// <param name="action">The action to perform</param>
        /// <param name="mode">The mode to perform in</param>
        /// <returns>Returns the result of attempting to add the binding to the collection</returns>
        public bool AddBindings(Keys key, string action, KeyMode mode)
        {
            return _keyboardBindings.AddBinding(key, action, mode);
        }

        /// <summary>
        /// Get the key for the given action
        /// </summary>
        /// <param name="action">The action to get the key for</param>
        /// <returns>The key that matches the action, or 0 if there is no key for the given action</returns>
        public Keys GetKey(string action)
        {
            return _keyboardBindings.GetKey(action);
        }

        /// <summary>
        /// Handle tick updates
        /// </summary>
        public void Update()
        {
            Dictionary<Keys, KeyBinding> bindings = _keyboardBindings.GetBindings();

            foreach (KeyValuePair<Keys, KeyBinding> b in bindings)
            {
                bool prevState;
                if (_previousKeyState.ContainsKey(b.Key))
                {
                    prevState = _previousKeyState[b.Key];
                }
                else
                {
                    prevState = false;
                }

                //held
                if (_inputCore.KeyPressed(b.Key) && prevState)
                {
                    KeyEventArgs args = new KeyEventArgs();
                    args.Key = b.Key;
                    args.Binding = b.Value;
                    OnKeyHeld(args);
                }

                //pressed
                if (_inputCore.KeyPressed(b.Key) && !prevState)
                {
                    KeyEventArgs args = new KeyEventArgs();
                    args.Key = b.Key;
                    args.Binding = b.Value;
                    OnKeyPressed(args);
                }

                //released
                if (!_inputCore.KeyPressed(b.Key) && prevState)
                {
                    KeyEventArgs args = new KeyEventArgs();
                    args.Key = b.Key;
                    args.Binding = b.Value;
                    OnKeyReleased(args);
                }

                //TODO: doublepressed

                //save the key state
                if (!_previousKeyState.ContainsKey(b.Key))
                {
                    _previousKeyState.Add(b.Key, _inputCore.KeyPressed(b.Key));
                }
                else
                {
                    _previousKeyState[b.Key] = _inputCore.KeyPressed(b.Key);
                }
            }
        }

        /// <summary>
        /// Save the key bindings out to the given file
        /// </summary>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool Save()
        {
            bool result = true;

            return result;
        }

        /// <summary>
        /// Passes the event arguments on to the delegate registered
        /// </summary>
        /// <param name="e">The key event arguments</param>
        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            if (KeyPressed != null)
            {
                KeyPressed(this, e);
            }
        }

        /// <summary>
        /// Passes the event arguments on to the delegate registered
        /// </summary>
        /// <param name="e">The key event arguments</param>
        protected virtual void OnKeyHeld(KeyEventArgs e)
        {
            if (KeyHeld != null)
            {
                KeyHeld(this, e);
            }
        }

        /// <summary>
        /// Passes the event arguments on to the delegate registered
        /// </summary>
        /// <param name="e">The key event arguments</param>
        protected virtual void OnKeyReleased(KeyEventArgs e)
        {
            if (KeyReleased != null)
            {
                KeyReleased(this, e);
            }
        }
    }

    /// <summary>
    /// The arguments for a key pressed event
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public KeyBinding Binding { get; set; }
    }
}
