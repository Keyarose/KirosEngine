namespace KirosProject
{
    class Gear
    {
        protected Vector3 _shaftDirection;
        protected float _radius;
        protected float _thickness;
        protected int _toothCount;
        protected ToothShape _toothShape;
        protected GearType _type;
    }
    
    public enum GearType
    {
        Internal,
        External
    }
    
    public enum ToothShape
    {
        Spur,
        Helical,
        Skew,
        Bevel,
        SpiralBevel,
        Hypoid,
        Crown,
        Worm
    }
}