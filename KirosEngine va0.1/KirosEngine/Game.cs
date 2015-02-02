using System;

namespace KirosEngine
{
    public abstract class Game
    {
        public virtual bool Load()
        {
            bool result = true;

            return result;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }
    
    /// <summary>
    /// Defines basic information for a game
    /// </summary>
    public struct GameData
    {
        private string _title;
        private string _iconPath;
        private string _copyright;
        
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        
        public string IconPath
        {
            get
            {
                return _iconPath;
            }
            set
            {
                _iconPath = value;
            }
        }
        
        public string Copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                _copyright = value;
            }
        }
    }
}