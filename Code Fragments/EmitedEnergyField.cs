namespace KirosPhysics
{
    public class EmitedEnergyField : EnergyField
    {
        protected Vector3 _emissionPoint;
        
        public Vector3 EmissionPoint
        {
            get
            {
                return _emissionPoint;
            }
        }
        
        public override float StrengthAtPoint(Vector3 point)
        {
        
        }
    }
}