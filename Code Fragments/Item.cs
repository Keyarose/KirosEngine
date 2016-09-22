public class Item
{
    private string _name;
    //base id is a unique identifier to distingush different items
    private string _baseID;
    //instance id is a unique identifier to distingush between different items of the same kind
    private string _instaceID;
    //what kind of item it is
    private string _itemType;
    //what kind of material the item is composed of(metal, wood, stone, plant, etc.)
    private string _materialType;
    private float _weight;
    private float _durability;
    private float _quality;
    //3d model for the item
    private Model _model;
    //texture for the item's model
    private Texture _modelTexture;
    //icon texture for the item
    private Bitmap _sprite;
    	
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
    
    public float Weight
    {
        get
        {
            return _weight;
        }
    }
    
    public float Durability
    {
        get
        {
            return _durability;
        }
    }
    
    public float Quality
    {
        get
        {
            return _quality;
        }
    }
    
    public string MaterialType
    {
        get
        {
            return _materialType;
        }
    }
    
    public Model Model
    {
        get
        {
            return _model;
        }
    }
    
    public Texture Texture
    {
        get
        {
            return _modelTexture;
        }
    }
    
    public Bitmap Sprite
    {
        get
        {
            return _sprite;
        }
    }
    	
    public Item(string name, string baseID, string itemType, string materialType, string xmlFile)
    {
        _name = name;
        _baseID = baseID;
        _itemType = itemType;
        _materialType = materialType;
        
        this.LoadItem(xmlFile);
    }
    
    //create item from xml file
    public Item(string xmlFile)
    {
        this.ParseXML(xml);
    }
    
    //create item from xml element
    public Item(XElement xml)
    {
        this.ParseXML(xml);
    }
    	
    //load the item from the given xml file
    public void LoadItem(string xmlFile)
    {
        this.ParseXML(xml);
    }
    	
    public static XElement Serialize(Item item)
    {
        return new XElement("Item",
        new XElement ("Name", item.Name),
        new XElement ("BaseID", item.BaseID),
        new XElement ("ItemType", item.ItemType),
        new XElement ("MaterialType", item.MaterialType),
        new XElement ("Weight", item.Weight)
        );
    }
    	
    private void ParseXML(XElement xml)
    {
        
    }
}