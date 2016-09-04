namespace KirosProject
{
    public class ActorMemory
    {
        protected List<Event> _eventList;
        protected string _actorID; //owning actor's id
        
        public string OwnerID
        {
            get
            {
                return _actorID;
            }
        }
        
        public ActorMemory(string actorID)
        {
            _actorID = actorID;
            _eventList = new List<Event>();
        }
        
        public ActorMemory(XElement xml)
        {
            //parse
        }
        
        public void AddEvent(Event event)
        {
            _eventList.Add(event);
        }
        
        //returns null if not found?
        public Event GetEvent(string eventID)
        {
            return _eventList.Find(x => x.ID.Equals(eventID));
        }
        
        //returns empty list if not found
        public List<Event> GetEventsInvolvingActor(string actorID)
        {
            List<Event> result = new List<Event>();
            
            foreach(Event e in _eventList)
            {
                if(e.InvolvesActor(actorID))
                {
                    result.Add(e);
                }
            }
            
            return result;
        }
    }
}