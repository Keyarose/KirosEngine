using System;

namespace KirosEngine
{
    public abstract class Game
    {
        protected string _gamePath;

        public virtual bool Load(string filePath)
        {
            bool result = true;
            _gamePath = filePath;
            ErrorLogger.Instance.FilePath = _gamePath;
            string gameDataFilePath = _gamePath + "/data.xml";

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