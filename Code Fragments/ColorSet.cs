namespace KirosEngine
{
    /// <summary>
    /// Contains a set of colors used by an object, can identify the color by a key
    /// </summar>
    class ColorSet
    {
        private Dictionary<string, Color4> _colors;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ColorSet()
        {
            _colors = new Dictionary<string, Color4>();
        }
        
        /// <summary>
        /// Adds the given color if the given key is not already in use
        /// </summary>
        /// <param name="color">The color to be added</param>
        /// <param name="key">The key for the color</param>
        /// <returns>True if the color is successfuly added, false otherwise</returns>
        public bool AddColor(Color4 color, string key)
        {
            if(!_colors.ContainsKey(key))
            {
                _colors.Add(key, color);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Change the color for the given key
        /// </summary>
        /// <param name="color">The color to change to</param>
        /// <param name="key">The key for the color</param>
        /// <returns>True if the key is in use and it successfuly changes the color, false otherwise</returns>
        public bool ChangeColor(Color4 color, string key)
        {
            if(_colors.ContainsKey(key))
            {
               _colors[key] = color;
               return true; 
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Get the color for the given key
        /// </summary>
        /// <param name="key">The key for the color</param>
        /// <returns>The color if the key is valid, null otherwise</returns>
        public Color4 GetColor(string key)
        {
            Color4 result;
            
            if(_colors.TryGetValue(key, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}