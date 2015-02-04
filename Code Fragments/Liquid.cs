namespace KirosEngine.Chemistry
{
    public class Liquid
    {
        private string _name;
        
        private float _density;
        private float _viscosity
        
        private float _boilingPoint;
        private float _meltingPoint;
        private Molecule _composition;
        private bool _solvant;
        
        private float _volume;
        
        public float Density
        {
            get
            {
                return _density;
            }
            set
            {
                _density = value;
            }
        }
        
        public float Viscosity
        {
            get
            {
                return _viscosity;
            }
            set
            {
                _viscosity = value;
            }
        }
        
        public float BoilingPoint
        {
            get
            {
                return _boilingPoint;
            }
            set
            {
                _boilingPoint = value;
            }
        }
        
        public float MeltingPoint
        {
            get
            {
                return _meltingPoint;
            }
            set
            {
                _meltingPoint = value;
            }
        }
    }
}