namespace KirosEngine.Chemistry
{
    public class Molecule
    {
        private string _name;
        
        private float _mass;
        private float _charge;//?
        
        private Atom[] _atoms;
        
        public Molecule(string name, int atomCount, string data)
        {
            _name = name;
            _atoms = new Atom[atomCount];
        }
        
        public Molecule(XElement moleculeXml)
        {
            
        }
        
        ///<summary>
        ///Performs a reaction with the given amount of energy
        ///</summary>
        ///<returns>ReactionResult containing the result of the reaction</returns>
        public ReactionResult Reaction(float energyInput)
        {
        
        }
        
        ///<summary>
        ///Serialize the molecule as xml data for writing
        ///</summary>
        ///<returns>XElement containing the xml data</returns>
        public XElement Serialize()
        {
        
        }
    }
}