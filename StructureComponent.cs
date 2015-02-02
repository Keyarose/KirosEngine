namespace KirosProject
{
    public abstract class StructureComponent
    {
        private Vector3 _bounding1;
        private Vector3 _bounding2;
        private Vector3 _translation;
        private string _compID;
        
        private ComponentType _type;
        private float _durability;
        private Vector3 _centerMass;
        
        /// <summary>
        /// Public accessor for the component's id
        /// </summary>
        public string ComponentID
        {
            get
            {
                return _compID;
            }
        }
        
        /// <summary>
        /// Public accessor for the component's position
        /// </summary>
        public Vector3 Translation
        {
            get
            {
                return _translation;
            }
            set
            {
                _translation = value;
            }
        }
        
        /// <summary>
        /// Public accessor for the component's durability
        /// </summary>
        public float Durability
        {
            get
            {
                return _durability;
            }
            set
            {
                _durability = value;
            }
        }
        
        /// <summary>
        /// Public accessor for the component's type
        /// </summary>
        public ComponentType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        
        /// <summary>
        /// Basic constructor for a StructureComponent
        /// </summary>
        /// <param name="bound1">First bounding point</param>
        /// <param name="bound2">Second bounding point</param>
        /// <param name="compID">ID of the StructureComponent</param>
        /// <param name="type">Type of the StructureComponent</param>
        public StructureComponent(Vector3 bound1, Vector3 bound2, string compID, ComponentType type)
        {
            _bounding1 = bound1;
            _bounding2 = bound2;
            _compID = compID;
            
            _type = type;
        }
        
        public StructureComponent(XElement xml)
        {
            this.ParseXml(xml);
        }
        
        private void ParseXml(XElement xml)
        {
        
        }
    }
    
    public enum ComponentType
    {
        Wall,
        Door,
        Window,
        Floor,
        Roof
    }
}