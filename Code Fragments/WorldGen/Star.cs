namespace KirosProject.WorldGen
{
    class Star
    {
        protected Orbit _orbit;
        protected float _mass;
        protected float _temperature;
        protected float _radius;
        protected float _density;
        protected float _luminosity;
        protected float _age;
        protected float _rotation;
        protected StellarClass _starClass;
        protected int _starClassNumber;
        
        
        public Star Generate(float seed)
        {
            Random rand = new Random(seed);
            int starClass = rand.Next(0, 7);
            
            switch(starClass)
            {
                case 0:
                {
                    _starClass = StellarClass.ClassO;
                    break;
                }
                
                case 1:
                {
                    _starClass = StellarClass.ClassB;
                    break;
                }
                
                case 2:
                {
                    _starClass = StellarClass.ClassA;
                    break;
                }
                
                case 3:
                {
                    _starClass = StellarClass.ClassF;
                    break;
                }
                
                case 4:
                {
                    _starClass = StellarClass.ClassG;
                    break;
                }
                
                case 5:
                {
                    _starClass = StellarClass.ClassK;
                    break;
                }
                
                case 6:
                {
                    _starClass = StellarClass.ClassM;
                    break;
                }
            }
            
            int starClass1 = rand.Next(0, 8);
            switch(starClass1)
            {
                case 0:
                {
                    _starClass = _starClass | StellarClass.Ia;
                    break;
                }
                
                case 1:
                {
                    _starClass = _starClass | StellarClass.Ib;
                    break;
                }
                
                case 2:
                {
                    _starClass =  _starClass | StellarClass.II;
                    break;
                }
                
                case 3:
                {
                    _starClass = _starClass | StellarClass.III;
                    break;
                }
                
                case 4:
                {
                    _starClass = _starClass | StellarClass.IV;
                    break;
                }
                
                case 5:
                {
                    _starClass = _starClass | StellarClas.V;
                    break;
                }
                
                case 6:
                {
                    _starClass = _starClass | StellarClass.VI;
                    break;
                }
                
                case 7:
                {
                    _starClass = _starClass | StellarClass.VII;
                    break;
                }
            }
        }
    }
    
    //should only select one value for 0 - 2^5 and one for 2^6 - 2^13
    [Flags]
    public enum StellarClass
    {
        ClassO = 0,
        ClassB = 1 << 0,
        ClassA = 1 << 1,
        ClassF = 1 << 2,
        ClassG = 1 << 3,
        ClassK = 1 << 4,
        ClassM = 1 << 5,
        Ia = 1 << 6,
        Ib = 1 << 7,
        II = 1 << 8,
        III = 1 << 9,
        IV = 1 << 10,
        V = 1 << 11,
        VI = 1 << 12,
        VII = 1 << 13
    }
}