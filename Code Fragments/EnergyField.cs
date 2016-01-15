namespace KirosPhysics
{
    //Defines an ambent energy field, could be electromag, thermal, grav, or fictonal
    public class EnergyField
    {
        protected string _name;
        protected float _strength;
        
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        public EnergyField(string name)
        {
            _name;
        }
        
        public EnergyField(XElement xml)
        {
            //parse xml
        }
        
        public virtual float StrengthAtPoint(Vector3 point)
        {
        
        }
    }
}