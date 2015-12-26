namespace KirosPhysics
{
    public class EmitedEnergyField : EnergyField
    {
        protected Vector3 _emissionPoint;
        protected float _fieldStrength; //defines a multiplyer for the strength of the field
        
        public Vector3 EmissionPoint
        {
            get
            {
                return _emissionPoint;
            }
        }
        
        public override float StrengthAtPoint(Vector3 point)
        {
            //options: compile at runtime, string parseing, script engine (best option) 
        }
        
        //TODO: define material and other field interactions
    }
}