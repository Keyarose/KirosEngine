using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using KirosEngine.Exception;

namespace KirosEngine.Input
{
    /// <summary>
    /// Collection of all the keys used by the loaded program and the action they are bound to
    /// </summary>
    class KeyboardBindings
    {
        private Dictionary<Keys, KeyBinding> _bindings;

        /// <summary>
        /// Base constructor
        /// </summary>
        public KeyboardBindings()
        {
            _bindings = new Dictionary<Keys, KeyBinding>();
        }

        /// <summary>
        /// Add a new key binding
        /// </summary>
        /// <param name="key">The key to bind to</param>
        /// <param name="action">The action to perform</param>
        /// <param name="mode">The mode to perform in</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool AddBinding(Keys key, string action, KeyMode mode)
        {
            if (!_bindings.ContainsKey(key))
            {
                _bindings.Add(key, new KeyBinding(action, mode));
                return true;
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed to add binding, key: {0} already in use.", key));
                return false;
            }
        }

        /// <summary>
        /// Change the binding of the given key
        /// </summary>
        /// <param name="key">The key to change the binding of</param>
        /// <param name="action">The new action</param>
        /// <param name="mode">The new mode</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool ChangeBinding(Keys key, string action, KeyMode mode)
        {
            if (_bindings.ContainsKey(key))
            {
                _bindings[key] = new KeyBinding(action, mode);
                return true;
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed to change binding, key: {0} not in use.", key));
                return false;
            }
        }

        /// <summary>
        /// Change the action of the binding for the given key
        /// </summary>
        /// <param name="key">The key to change the binding of</param>
        /// <param name="action">The new action</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool ChangeBindingAction(Keys key, string action)
        {
            if (_bindings.ContainsKey(key))
            {
                _bindings[key].Action = action;
                return true;
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed to change binding, key: {0} not in use.", key));
                return false;
            }
        }

        /// <summary>
        /// Change the mode of the binding for the given key
        /// </summary>
        /// <param name="key">The key to change the binding of</param>
        /// <param name="mode">The new mode</param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool ChangeBindingMode(Keys key, KeyMode mode)
        {
            if (_bindings.ContainsKey(key))
            {
                _bindings[key].Mode = mode;
                return true;
            }
            else
            {
                ErrorLogger.Write(String.Format("Failed to change binding, key: {0} not in use.", key));
                return false;
            }
        }

        /// <summary>
        /// Get the binding for the given key
        /// </summary>
        /// <param name="key">Key to get the binding for</param>
        /// <returns>Returns the binding for the given key, or null if there is no binding</returns>
        public KeyBinding GetBinding(Keys key)
        {
            if (_bindings.ContainsKey(key))
            {
                return _bindings[key];
            }
            return null;
        }

        /// <summary>
        /// Get the key for the given action
        /// </summary>
        /// <param name="action">The given action</param>
        /// <returns>The key for the given action, or 0 if there is no registered key</returns>
        public Keys GetKey(string action)
        {
            //returns the first key registered for the given action
            var key = _bindings.FirstOrDefault(x => x.Value.Action.Equals(action)).Key;

            //if key is 0 its returning the default value
            return key;
        }

        /// <summary>
        /// Get all the bindings
        /// </summary>
        /// <returns>Returns a dictionary containing all the bindings</returns>
        public Dictionary<Keys, KeyBinding> GetBindings()
        {
            return _bindings;
        }

        /// <summary>
        /// Read all the bindings from the given file
        /// </summary>
        /// <param name="file">The file to read from</param>
        /// <exception cref="InvalidDataValueException">Thrown when the data value read in is not of the expected type</exception>
        public void ReadBindingsFromFileXML(string file)
        {
            //TODO: process xml
        }

        /// <summary>
        /// Write the current bindings to the given file location
        /// </summary>
        /// <param name="file">The file to write to</param>
        /// <returns>Returns false if the process fails, true if it completes</returns>
        public bool SaveBindings(string file)
        {
            bool result = false;

            //TODO: write bindings to file

            return result;
        }
    }

    /// <summary>
    /// Defines a single key binding
    /// </summary>
    public class KeyBinding
    {
        private string _action;
        private KeyMode _mode;

        /// <summary>
        /// Base constructor for the key binding
        /// </summary>
        /// <param name="action">The action to perform when the key is used</param>
        /// <param name="mode">The method in which to activate the key</param>
        public KeyBinding(string action, KeyMode mode)
        {
            _action = action;
            _mode = mode;
        }

        /// <summary>
        /// Public accessor for the key binding's action
        /// </summary>
        public string Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Public accessor for the key binding's mode
        /// </summary>
        public KeyMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }
    }

    /// <summary>
    /// Defines the different modes for key useage
    /// </summary>
    public enum KeyMode
    {
        Pressed,
        Released,
        Held,
        DoublePressed //requires some form of timer
    }
}
