namespace KirosEngine
{
    public class ClickTriangle : ClickBox
    {
        protected Vector2 _coord3;
        
        public ClickTriangle(Vector2 coord1, Vector2 coord2, Vector3 coord3) : base(coord1, coord2)
        {
            _coord3 = coord3;
        }
        
        public override bool ClickCheck(Vector2 coords)
        {
            if(_enabled)
            {
                //get the coord with the smallest x value, and the one with the largest
                //
            }
            else
            {
                return false;
            }
        }
    }
}