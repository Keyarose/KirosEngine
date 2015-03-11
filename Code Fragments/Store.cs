namespace KirosProject
{
    public class Store
    {
        struct ItemPrice
        {
            private Item _item;
            private float _price;
            
            public Item Item
            {
                get
                {
                    return _item;
                }
            }
            
            public float Price
            {
                get
                {
                    return _price;
                }
                set
                {
                    _price = value;
                }
            }
            
            public ItemPrice(Item item, float price)
            {
                _item = item;
                _price = price;
            }
        }
        //the id for the entity that represents the store's owner
        protected string _ownerEntityID;
        protected string _name;
        //the id for the structure that is the store in the game world
        protected string _structureID;
        
        protected TradeMethod _tradeMethod; //will depend on the culture of the area and owner
        protected float _stockSpace; //cubic volume availabe to store items, calculated using structure
        protected float _usedStockSpace;
        protected List<ItemPrice> _stock; //can include storage items ie. crates(crate IS-A item)
        
        /// <summary>
        /// Public readonly accessor for the store's stock list
        /// </summary>
        public List<ItemPrice>.Enumerator StockList
        {
            get
            {
                _stock.GetEnumerator();
            }
        }
        
        /// <summary>
        /// Public accessor for the store's trading method
        /// </summary>
        public TradeMethod TradeType
        {
            get
            {
                return _tradeMethod;
            }
            set
            {
                _tradeMethod = value;
            }
        }
        
        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <params name="ownerID">The entity ID of the owner of the store</params>
        /// <params name="storeName">The name of the store</params>
        /// <params name="storeStructure">The ID of the structure that the store is in</params>
        public Store(string ownerID, string storeName, string storeStructure)
        {
            _ownerID = ownerID;
            _name = storeName;
            _structureID = storeStructure;
            _stock = new List<ItemPrice>();
            
            //TODO: calculate stock space
        }
        
        /// <summary>
        /// Constructor for initalization from xml data
        /// </summary>
        public Store(XElement xml)
        {
            this.ParseXml(xml);
        }
        
        /// <summary>
        /// Buy an item from a customer for another item
        /// </summary>
        /// <params name="item">The  item being bought by the store</params>
        /// <params name="price">The item the seller is asking for in return</params>
        /// <returns>True if the trade is successful, false otherwise</returns>
        public virtual bool BuyItem(Item item, Item price)
        {
            //should check if 1. the store has the price item in stock, and 2. the store has space for the item being bought
        }
        
        /// <summary>
        /// Buy an item from a customer for the amount of currency given
        /// </summary>
        /// <params name="item">The item being bought by the store</params>
        /// <params name="price">The amount of currency being traded for the item</params>
        /// <returns>True if the trade is successful, false otherwise</returns>
        public virtual bool BuyItem(Item item, float price)
        {
            
        }
        
        /// <summary>
        /// Gets the price for the given item
        /// </summary>
        /// <params name="item">The item to get the price for</params>
        /// <returns>The price of the item as a float</returns>
        public virtual float PriceForItem(Item item)
        {
            var items = 
        }
        
        /// <summary>
        /// Tries to sell the given item to the customer
        /// </summary>
        /// <params name="item">The item being sold</params>
        /// <returns>True if successful, false otherwise</returns>
        public virtual bool SellItem(Item item)
        {
            
        }
        
        /// <summary>
        /// Checks to see if the store has stock space for the given item.
        /// </summary>
        /// <params name="item">The item to check the space for.</params>
        /// <returns>True if there is space, false otherwise.</returns>
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
        /// and if it has room to add an item. Should be called before HasStockSpaceForItem, unless the item
        /// is a storage item of the same type
        /// </summary>
        /// <params name="item">The item to perform the check for.</params>
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
        
        protected virtual bool ParseXml(XElement xml)
        {
        
        }
    }
    
    public enum TradeMethod
    {
        Barter,
        Gift,
        Exchange //ie. money
    }
}