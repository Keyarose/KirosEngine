public class Entity
{
	private Model _model;
	private Texture _texture;
	
	private Vector3 _position;
	private Vector3 _direction; //direction the entity faces
	
	//the identifier string for the entity
	private string _id;
	
	/// <summary>
	/// Public accessor for the Entity's position
	/// </summary>
	public Vector3 Position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}
	
	/// <summary>
	/// Public accessor for the Entity's direction
	/// </summary>
	public Vector3 Direction
	{
		get
		{
			return _position;
		}
		set
		{
			_direction = value.normalize;
		}
	}
	
	/// <summary>
	/// Public accessor for the Entity's ID
	/// </summary>
	public string ID
	{
		get
		{
			return _id;
		}
	}
	
	//=============================================
	// Methods
	//=============================================
	#region Methods
	public Entity(Model model, Texture texture, Vector3 position)
	{
		_model = model;
		_texture = texture;
		_position = position;
		
		_id = this.GetNewID();
	}
	
	/// <summary>
	/// Load the Entity from the given xml document
	/// </summary>
	/// <param name="xml">The XDocument the Entity is loaded from</param>
	public void LoadFromXML(XDocument xml)
	{
		
	}
	
	/// <summary>
	/// Change the Entity's model to the given one
	/// </summary>
	/// <param name="newModel">The new model</param>
	public void ChangeModel(Model newModel)
	{
		_model = newModel;
	}
	
	/// <summary>
	/// Check to see if the given ID is alredy in use
	/// </summary>
	/// <param name="id">The id string to be checked</param>
	/// <returns>True if the id is already in use, false otherwise</returns>
	private bool CheckID(string id)
	{
		
	}
	
	private string GetNewID()
	{
		//generate the id somehow
		string newID;
		
		while(this.CheckID(newID))
		{
			//generate another id
		}
		
		return newID;
	}
	
	#endregion 
}