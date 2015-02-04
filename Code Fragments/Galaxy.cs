namespace KirosProject
{
    //represents a galactic map
    public class Galaxy
    {
        private string _name;
        private Vector3  _location;
        
        private List<Star> _stars;
        private List<Structure> _structures; //structures in interstellar space, not in orbit of a star
        private List<StellarObject> _sObjects; //black holes, nebulas, pulsars, wormholes
        
        public Galaxy(string name, Vector3 location)
        {
            _name = name;
            _location = location;
            
            _stars = new List<Star>();
            _structures = new List<Structure>();
            _sObjects = new List<StellarObject>();
        }
        
        //construct a galaxy from xml data
        public static Galaxy(XDocument xml)
        {
            //TODO: process xml
            Galaxy newGalaxy = new Galaxy(/*#xml.name, xml.location#*/);
            //TODO: fill stars, structures, and stellar objects from xml
            return newGalaxy;
        }
        
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        public Vector3 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        
        public void AddStar(Star star)
        {
            _stars.Add(star);
        }
        
        public void AddStructure(Structure structure)
        {
            _structures.Add(structure);
        }
        
        public void AddStellarObject(StellarObject sObject)
        {
            _sObjects.Add(sObject);
        }
        
        //looks for a star using the given name if there is no result return null
        public Star GetStarByName(string name)
        {
            foreach (Star s in _stars)
            {
                if (s.Name.Equals(name, CurrentCultureIgnoreCase))
                {
                    return s;
                }
            }
            //no matches
            return null;
        }
        
        //looks for a star using the given id if there is no result return null
        public Star GetStarByID(string id)
        {
            foreach (Star s in _stars)
            {
                if (s.ID.Equals(id))
                {
                    return s;
                }
            }
            //no match
            return null;
        }
        
        public Star GetStarByLocation(Vector3 location)
        {
            foreach (Star s in _stars)
            {
                if (s.Location.Equals(location))
                {
                    return s;
                }
            }
            //no match
            return null;
        }
        
        public List<Star> GetStarsNear(Vector3 location, float range)
        {
            List<Star> sList = new List<Star>();
            
            foreach (Star s in _stars)
            {
                if (s.Location.Distance(location) <= range)
                {
                    sList.Add(s);
                }
            }
            
            if (sList.Count != 0)
            {
                return sList;
            }
            else
            {
                //no matches
                return null;
            }
        }
        
        public Structure GetStructure(string id)
        {
        
        }
        
        public StellarObject GetStellarObject(string id)
        {
        
        }
        
        public XElement Serialize()
        {
            //TODO: serialize the data into xml
            //stars, structures, and stellars should be represented by ids
        }
    }
}