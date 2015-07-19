namespace KirosPhysics
{
    public class EnergyField
    {
        protected string _name;
        
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