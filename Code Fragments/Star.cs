using SlimDX;
using KirosPhysics.Astronomy;

namespace KirosProject
{
    //represents a star on a map
    public class Star : Astronomy.Star
    {
        private string _name;
        private string _id; //should be ccreated on generation and registration, unique
        private Vector3 _location; //0,0,0 is relative to the galaxy the star is in
        
        private float _tickTime; //records the last time a tick was performed
        
        public Star(string name, Vector3 location, Astronomy.Star star) : base(star)
        {
            _name = name;
            _location = location;
        }
        
        public Star(string name, string id, Vector3 location, Astronomy.Star star) : base(star)
        {
            _name = name;
            _id = id;
            _location = location;
        }
        
        //randomly generate a star
        public static Star GenerateStar()
        {
            //TODO: access star data table
            //TODO: generate the data and create a star instance
        }
        
        //generate a star using a seed for the random
        public static Star GenerateStar(int seed)
        {
            //TODO: generate and choose values
        }
        
        public string Name()
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
        
        public string ID()
        {
            get
            {
                return _id;
            }
        }
        
        public Vector3 Location()
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
        
        public Orbit Orbit()
        {
            get
            {
                return _orbit;
            }
            set
            {
                _orbit = value;
            }
        }
        
        public float Radius()
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }
        
        public float Mass()
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = value;
            }
        }
        
        public float Temperature()
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value;
            }
        }
        
        public float Age()
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
        
        //Advances the star's state in terms of its age
        public void Tick(/*Time t*/)
        {
            float timePassed = t - _tickTime;
            //TODO: perform the tick and update the stats
        }
        
        //serialize the star's info into xml format
        public XElement Serialize()
        {
            //TODO: serialize the data
        }
    }
}