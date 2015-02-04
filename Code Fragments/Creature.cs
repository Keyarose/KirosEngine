namespace KirosProject
{
    /// <summary>
    /// Abstract base class for animals, monsters, and others
    /// </summary>
    public abstract class Creature
    {
        private string _name;
        private string _baseID;
        private string _instanceID;
        
        private Model _model;
        private AnimationSet _animationSet;
        private Light[] _lights;
        
        private string _species;
        private float _age;
        private GeneticCode _genetics;
        
        /// <summary>
        /// The creature's name, (could be generic species name with a number ie. Ork3)
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
        /// The instance id of the creature, registered by the engine
        /// </summary>
        public string InstanceID
        {
            get
            {
                return _instanceID;
            }
        }
        
        /// <summary>
        /// The name of the species of the creature
        /// </summary>
        public string Species
        {
            get
            {
                return _species;
            }
            set 
            {
                _species = value;
            }
        }
        
        /// <summary>
        /// The age of the creature as a float
        /// </summary>
        public float Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
    }
}