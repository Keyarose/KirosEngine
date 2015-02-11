namespace KirosProject
{
    public class Store
    {
        //the id for the entity that represents the store's owner
        protected string _ownerEntityID;
        protected string _name;
        //the id for the structure that is the store in the game world
        protected string _structureID;
        
        protected TradeMethod _tradeMethod;
        protected float _stockSpace; //cubic volume availabe to store items, calculated using structure
        protected List<Item> _stock;
        
        public enum TradeMethod
        {
            Barter,
            Gift,
            Exchange //ie. money
        }
    }
}