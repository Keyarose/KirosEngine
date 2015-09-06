public class UIContainer : UIComponent
{
    public UIContainer(Vector2 position, float width, float height) : base(position, width, height)
    {
    
    }
    
    public override Draw(bool debug)
    {
        if(debug)
        {
            
        }
        
        base.Draw();
    }
}