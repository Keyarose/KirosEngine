namespace KirosProject
{
    class Event
    {
        protected string _id;
        protected string _masterID;
        protected TimeCode _time; //defines when the event was in terms of the world
        //list of actions grouped with the performing actor's id and an unique id number
        protected List<Triad<uint, string, Action>> _actorActionList;
        //string == "null" refers to unknown actor
        protected uint _nextActionID;
        
        public string ID
        {
            get
            {
                return _id;
            }
        }
        
        public string MasterID
        {
            get
            {
                return _masterID;
            }
        }
        
        public int ActionCount
        {
            get
            {
                return _actorActionList.Count();
            }
        }
        
        public Event(string id)
        {
            _id = id;
            _actorActionList = new List<Triad<uint, string, Action>>();
            _nextActionID = 0;
        }
        
        ///<summary>
        /// Add an action along with a performing actor to the event
        ///</summary>
        ///<param name="actorID">ID for the actor performing the action</param>
        ///<param name="action">The action being added</param>
        public void AddAction(string actorID, Action action)
        {
            _actorActionList.Add(new Triad<uint, string, Action>(_nextActionID, actorID, action));
            _nextActionID++;
        }
        
        ///<summary>
        /// Get a list of actors involved in the event
        ///</summary>
        public List<Actor> GetActorsInEvent()
        {
            List<Actor> result = new List<Actor>();
            
            foreach(Triad<uint, string, Action> p in _actorActionList)
            {
                result.add(ActorManager.GetActor(p.Item2));
            }
            
            return result;
        }
        
        public bool InvolvesActor(string actorID)
        {
            bool result false;
            
            foreach(Triad<uint, string, Action> t in _actorActionList)
            {
                if(t.Item2.Equals(actorID))
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }
    }
}