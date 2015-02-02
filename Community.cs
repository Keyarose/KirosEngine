namespace KirosProject
{
    public class Community
    {
        private string _name;
        private string _id;
        private List<Structure> _structures;
        
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
        
        public string ID
        {
            get
            {
                return _id;
            }
        }
        
        public Community(string id, string name)
        {
            _id = id;
            _name = name;
        }
        
        public bool AddStructure(Vector3 location, Structure structure, bool distanceCheck)
        {
            //check that the new structure isnt too close to others
            bool good = true;
            if(distanceCheck)
            {
                foreach(Structure s in _structures)
                {
                    if(location.Difference(s.Location) < s.MinimumBoundary)
                    {
                        good = false;
                        //message strucutre is too close to structure s, give min boundary
                        break;
                    }
                }
            }
            
            return good;
        }
        
        public Structure GetStructure(string id)
        {
            Structure result = null;
            
            foreach(Structure s in _structures)
            {
                if(s.InstanceID.Equals(id))
                {
                    result = s;
                }
            }
            
            return result;
        }
    }
}