namespace KirosEngine
{
    public class ClickBox
    {
        protected Vector2 _coord1, _coord2;
        
        protected bool _enabled = true;
        
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
        
        public ClickBox(Vector2 coord1, Vector2 coord2)
        {
            _coord1 = coord1;
            _coord2 = coord2;
        }
        
        public virtual bool ClickCheck(Vector2 coords)
        {
            if(_enabled)
            {
                if(coords.X > _coord1.X && coords.X < _coord2.X)
                {
                    if(coords.Y > _coord1.Y && coords.Y < _coord2.Y)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}