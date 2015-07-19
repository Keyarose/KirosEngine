namesapce KirosPhysics
{
    public interface IRadioactive
    {
        protected float _halfLife;
        protected EmissionType _emissionType;
        protected Particle _decayResult;
        
        public float HalfLife
        {
            get
            {
                return _halfLife;
            }
        }
        
        public EmissionType Emission
        {
            get
            {
                return _emissionType;
            }
        }
        
        public Particle DecayResult
        {
            get
            {
                return _decayResult;
            }
        }
    }
    
    public enum EmissionType
    {
        Proton,
        Electron,
        Photon
    }
}