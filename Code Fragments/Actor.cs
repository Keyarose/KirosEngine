namespace KirosProject
{
    public class Actor
    {
        private String _id;
        protected ActorMemory _memory;
        protected Event _activeEvent;
        protected ?StackList?<Objective> _goals; //bottom of the stack is the primary goal, each goal on the stack above that is a midway goal to the primary.  These might not require completion in order hence the list component. ?add paralle objectives?
        
        public Actor(string id)
        {
            _id = id;
            _memory = new ActorMemory();
        }
        
        //Constructor for creating from xml
        public Actor(XElement xml)
        {
            //parse
        }
        
        public void SetObjective(Objective goal)
        {
            
        }
    }
}