public abstract class UIComponent
{
    protected ArrayList<UIComponent> _subComponents;
    
    protected Vector2 _position;
    protected float _width;
    protected float _height;
    
    public Vector2 Position
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
    
    public float Width
    {
        get
        {
            return _width;
        }
        set
        {
            _width = value;
        }
    }
    
    public float Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
        }
    }
    
    public UIComponent(Vector2 position, float width, float height)
    {
        _position = position;
        _width = width;
        _height = height;
    }
    
    public ArrayList<UIComponent> GetSubComponents()
    {
        return _subComponents;
    }
    
    public void AddSubComponent(UIComponent component)
    {
        _subComponents.Add(component);
    }
    
    public virtual void Draw()
    {
        //do draw for this
        
        foreach(UIComponent u in _subComopnents)
        {
            u.Draw();
        }
    }
}