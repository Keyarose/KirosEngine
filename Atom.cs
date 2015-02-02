namespace KirosEngine.Chemistry
{
    public class Atom
    {
        private string _name;
        private int _atomicNumber;
        private float _mass;
        private float _charge;
        
        private ElectronOrbits _electrons;
        
        public int AtomicNumber
        {
            get
            {
                return _atomicNumber;
            }
        }
        
        public float Mass
        {
            get
            {
                return _mass;
            }
        }
        
        public int NeutronCount
        {
            get
            {
                return _mass - _atomicNumber;
            }
        }
        
        public ElectronShell OuterShell
        {
            get
            {
                return _electrons.OuterShell;
            }
        }
    }
}