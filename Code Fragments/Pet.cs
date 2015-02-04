public class Pet
{
	private string _name;
	private Model _model;
	private Texture _texture;
	private List<Effect> _effects;
	private AnimationSet _animations;
	
	private BehaviourSet _behaviours;
	private RelationSet _relations;
	private string _owner;
	
	public Pet()
	{
		_effects = new List<Effect>();
		_animations = new AnimationSet();
		_behaviours = new BehaviourSet();
	}
	
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
	
	public string Owner
	{
		get
		{
			return _owner;
		}
	}
	
	public void SetModel(Model model)
	{
		_model = model;
	}
	
	public void SetTexture(Texture texture)
	{
		_texture = texture;
	}
	
	public void AddEffect(Effect effect)
	{
		_effects.add(effect);
	}
}