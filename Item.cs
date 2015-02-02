public class Item
{
    private string _name;
    //base id is a unique identifier to distingush different items
    private string _baseID;
    //instance id is a unique identifier to distingush between different items of the same kind
    private string _instaceID;
    private string _itemType;
    private string _materialType;
    private float _weight;
    private float _durability;
    private float _quality;
    private Model _model;
    //private (type?) _texture;
    //private (type?) _sprite;
    	
    public string Name
    {
        get
        {
        	    return _name;
        }
    }
    	
    public string BaseID
    {
        get
        {
            return _baseId;
        }
    }
    
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
    	
    public Item(string name, string baseID, string itemType, string materialType)
    {
        _name = name;
        _baseID = baseID;
        _itemType = itemType;
        _materialType = materialType;
    }
    	
    //load the item from the given xml file
    public void LoadItem(string xmlFile)
    {
        this.ParseXML(xml);
    }
    	
    public static XElement Serialize(Item item)
    {
        return new XElement("Item",
        new XElement ("name", item.Name),
        );
    }
    	
    private void ParseXML(XElement xml)
    {
        
    }
}