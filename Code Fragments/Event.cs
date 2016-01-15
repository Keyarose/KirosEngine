namespace KirosProject
{
    class Event
    {
        protected string _id;
        //list of actions grouped with the performing actor's id and an unique id number
        protected List<Triad<uint, string, Action>> _actorActionList;
        //string == "null" refers to unknown actor
        
        public string ID
        {
            get
            {
                return _id;
            }
        }
        
        public Event(string id)
        {
            _id = id;
            _actorActionList = new List<Pair<string, Action>>();
        }
        
        ///<summary>
        /// Add an action along with a performing actor to the event
        ///</summary>
        ///<param name="actorID">ID for the actor performing the action</param>
        ///<param name="action">The action being added</param>
        public void AddAction(string actorID, Action action)
        {
            _actorActionList.Add(new Pair<string, Action>(actorID, action));
        }
        
        ///<summary>
        /// Get a list of actors involved in the event
        ///</summary>
        public List<Actor> GetActorsInEvent()
        {
            List<Actor> result = new List<Actor>();
            
            foreach(Pair<string, Action> p in _actorActionList)
            {
                result.add(ActorManager.GetActor(p.Item1));
            }
            
            return result;
        }
    }
}