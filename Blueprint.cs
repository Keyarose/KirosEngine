namespace KirosProject
{
    public class BluePrint
    {
        private string _id;
        private string _name;
        private List<ComponentAlignment> _components;
        private string _itemIdResult;
        
        public BluePrint(string id, string name, string result)
        {
            _id = id;
            _name = name;
            _itemIdResult = result;
        }
        
        public BluePrint(XDocument xml)
        {
            this.ParseXml(xml);
        }
        
        private void ParseXml(XDocument xml)
        {
        
        }
        
        public void AddComponent(ComponentAlignment comp)
        {
            _components.Add(comp);
        }
        
        public void AddComponent(string itemType, string materialType, Vector3 position, Vector3 rotation)
        {
            _components.Add(new ComponentAlignment(itemType, materialType, position, rotation));
        }
        
        public List<ComponentAlignment> GetComponents()
        {
            return _components;
        }
        
        public string GetResult()
        {
            return _itemIdResult;
        }
    }
    
    struct ComponentAlignment
    {
        private string _itemType;
        private string _materialType;
        private Vector3 _position;
        private Vector3 _rotation;
        
        public string ItemType
        {
            get
            {
                return _itemType;
            }
        }
        
        public string MaterialType
        {
            get
            {
                return _materialType;
            }
        }
        
        public Vector3 Position
        {
            get
            {
                return _position;
            }
        }
        
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
        }
        
        public ComponentAlignment(string itemType, string matType, Vector3 position, Vector3 rotation)
        {
            _itemType = itemType;
            _materialType = matType;
            _position = position;
            _rotation = rotation;
        }
    }
}