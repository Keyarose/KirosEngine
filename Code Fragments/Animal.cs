namespace KirosProject
{
    /// <summary>
    /// Defines an animal sub class of creature
    /// </summary>
    public class Animal : Creature
    {
        private Biome[] _naturalEnviroment;
        
        /// <summary>
        /// An array of biomes representing the natural enviroments the animal lives in
        /// </summary>
        public Biome[] NaturalEnviroment
        {
            get
            {
                return _naturalEnviroment;
            }
            set
            {
                _naturalEnviroment = value;
            }
        }
        
        #region Constructors
        /// <summary>
        /// Load an animal from the given xml
        /// </summary>
        public Animal(XElement xml)
        {
            this.ParseXml(xml);
        }
        
        /// <summary>
        /// Create an animal with a baseID, model, animation set, and genetic code
        /// </summary>
        public Animal(string baseID, Model model, AnimationSet aniSet, GeneticCode genetic)
        {
            this._baseID = baseID;
            this._model = model;
            this._animationSet = aniSet;
            this._geneticCode = genetic;
        }
        
        /// <summary>
        /// Create an animal with a baseID, model, animantion set, light array, and genetic code
        /// </summary>
        public Animal(string baseID, Model model, AnimationSet aniSet, Light[] lights, GeneticCode genetic)
        {
            this._baseID = baseID;
            this._model = model;
            this._animationSet = aniSet;
            this._lights = lights
            this._geneticCode = genetic;
        }
        #endregion
        
        /// <summary>
        /// parse the xml and load the infomantion for the animal
        /// </summary>
        private bool ParseXml(XElement xml)
        {
        
        }
    }
}