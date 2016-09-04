namespace KirosProject
{
    public class Objective
    {
        protected string _description;
        protected ObjectiveType _type;
        protected List<Item> _itemsInvolved;
        protected List<string> _actorsInvolved;
        
        public string Description
        {
            get
            {
                return _description;
            }
        }
        
        public ObjectiveType Type
        {
            get
            {
                return _type;
            }
        }
        
        public Objective(ObjectiveType type)
        {
            _type = type;
        }
        
        public Objective(ObjectiveType type, string description)
        {
            _type = type;
            _description = description;
        }
        
        public Objective(XElement xml)
        {
            //parse
        }
        
        public enum OjbectiveType
        {
            PerformAction,
            ObtainItem,
            GiveItem,
            Reputation //interaction +/- change
        }
    }
}