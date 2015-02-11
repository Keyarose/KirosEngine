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
        protected float _usedStockSpace;
        protected List<Item> _stock; //can include storage items ie. crates(crate IS-A item)
        
        /// <summary>
        /// Checks to see if the store has stock space for the given item
        /// </summary>
        /// <params name="item">The item to check the space for</params>
        /// <returns>True if there is space, false otherwise</returns>
        protected virtual bool HasStockSpaceForItem(Item item)
        {
            if(_stockSpace == _usedStockSpace)
            {
                return false;
            }
            
            if(_stockSpace - _usedStockSpace < item.Volume)
            {
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Checks to see if the store has an item in stock that stores the type of the given item
        /// and if it has room to add an item.
        /// </summary>
        /// <returns>The index of the storage item if found, -1 if one is not found.</returns>
        protected virtual int HasStorageForItem(Item item)
        {
            for(int i = 0; i < _stock.Count; i++)
            {
                if(_stock[i] is ItemWithStorage)
                {
                    if(_stock[i].StoredItemType == item.Type && _stock[i].HasRoom)
                    {
                        return i;
                    }
                }
            }
            
            return -1;
        }
    }
    
    public enum TradeMethod
    {
        Barter,
        Gift,
        Exchange //ie. money
    }
}