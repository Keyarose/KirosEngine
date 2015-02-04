namespace KirosProject
{
    public class Structure
    {
        private List<StructureComponent> _structureComponents;
        private Vector3 _location;
        private float _minBoundary; //min distance another structure can be from it when owned by someone else
        private string _instanceID;
        
        public Vector3 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        
        public string InstanceID
        {
            get
            {
                return _instanceID;
            }
        }
        
        public float MinimumBoundary
        {
            get
            {
                return _minBoundary;
            }
            set
            {
                _minBoundary = value;
            }
        }
        
        public Structure(string instanceID, Vector3 location)
        {
            this._instanceID = instanceID;
            this._location = location;
        }
        
        public Structure(string instanceID, Vector3 location, List<StructureComponent> structureComps)
        {
            this._instanceID = instanceID;
            this._location = location;
            this._structureComponents = structureComps;
        }
        
        public Structure(XElement xml)
        {
            this.ParseXml(xml);
        }
        
        public void AddComponent(StructureComponent structComp)
        {
            _structureComponents.Add(structComp);
        }
        
        public StructureComponent GetComponent(string compID)
        {
            return _structureComponents.Find(x => x.ComponentID.Equals(compID));
        }
        
        private void ParseXml(XElement xml)
        {
        
        }
        
        public virtual void Draw()
        {
            foreach(StructureComponent s in _structureComponents)
            {
                _structureComponents.Draw();
            }
        }
    }
}