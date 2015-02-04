namespace KirosProject
{
    /// <summary>
    /// Defines a biome for a world's layout
    /// </summary>
    class Biome
    {
        private string _name;
        private float _adverageTemp;
        private float _humidity;
        private float _adveragePrecip;
        private Gas _atmosphere;
        
        private ColorSet _biomeColors;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Biome()
        {
            _biomeColors = new ColorSet();
        }
        
        /// <summary>
        /// Create a biome from information storred in xml
        /// </summary>
        /// <param name="xml">The xml to get the information from</param>
        public Biome(XElement xml)
        {
            _biomeColors = new ColorSet();
            
            this.ParseXml(xml);
        }
        
        /// <summary>
        /// The name of the biome type
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        /// <summary>
        /// The adverage temperature of the biome, will be modified by time of day, season, weather
        /// </summary>
        public float AdverageTemperature
        {
            get
            {
                return _adverageTemp;
            }
            set
            {
                _adverageTemp = value;
            }
        }
        
        /// <summary>
        /// the humidity in the biome, modified by time of day, season, weather
        /// </summary>
        public float Humidity
        {
            get
            {
                return _humidity;
            }
            set
            {
                _humidity = value;
            }
        }
        
        /// <summary>
        /// The adverage precipitation for the biome
        /// </summary>
        public float AdveragePrecipitation
        {
            get
            {
                return _adveragePrecip;
            }
            set
            {
                _adveragePrecip = value;
            }
        }
        
        /// <summary>
        /// The composition of the biome's atmosphere
        /// </summary>
        public Gas Atmosphere
        {
            get
            {
                return _atmosphere;
            }
            set
            {
                _atmosphere = value;
            }
        }
        
        //parse the given xml into the data structures
        private bool ParseXml(XElement xml)
        {
            //TODO: define the format for the xml and complete the parse function
        }
        
        /// <summary>
        /// Add the given color for the identifing key ex. key grassColor1
        /// </summary>
        public bool AddBiomeColor(Color4 color, string key)
        {
            return _biomeColors.AddColor(color, key);
        }
    }
}