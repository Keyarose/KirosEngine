namespace KirosProject
{
    public class Monster : Creature
    {
        private Biome[] _enviroment;
        private List<ItemRate> _dropList;
        
        private float _health;
        private float _defence; //used in calculating damage when attacked
        private float _strength; //used in calculating attack damage
        
        /// <summary>
        /// The list of biomes the monster can naturally spawn in
        /// </summary>
        public Biome[] NaturalEnviroment
        {
            get
            {
                return _enviroment;
            }
            set
            {
                _enviroment = value;
            }
        }
        
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
        
        #region Constructors
        /// <summary>
        /// Load the monster from xml data
        /// </summary>
        public Monster(XElement xml)
        {
            _dropList = List<ItemRate>();
            this.ParseXml(xml);
        }
        
        /// <summary>
        /// Create a monster with a base id, model, animation set, and genetic code
        /// </summary>
        public Monster(string baseID, Model model, AnimationSet aniSet, GeneticCode genetic)
        {
            _dropList = List<ItemRate>();
            this._baseID = baseID;
            this._model = model;
            this._animationSet = aniSet;
            this._geneticCode = genetic;
        }
        
        /// <summary>
        /// Create a monster with a base id, model, animation set, light array, and genetic code
        /// </summary>
        public Monster(string baseID, Model model, AnimationSet aniSet, Light[] lights, GeneticCode genetic)
        {
            _dropList = List<ItemRate>();
            this._baseID = baseID;
            this._model = model;
            this._animationSet = aniSet;
            this._lights = lights;
            this._geneticCode = genetic;
        }
        #endregion
        
        private bool ParseXml(XElement xml)
        {
            
        }
        
        /// <summary>
        /// Add the item to the drop list with the given rate
        /// </summary>
        public void AddToDropList(Item item, float rate)
        {
            this.AddToDropList(new ItemRate(item, rate));
        }
        
        /// <summary>
        /// Add the item rate to the drop list
        /// </summary>
        public void AddToDropList(ItemRate itemRate)
        {
            _dropList.Add(itemRate);
        }
        
        /// <summary>
        /// Compile the list of items to drop
        /// </summary>
        private List<Item> Drop()
        {
            Random rand = new Random();
            List<Item> drops = new List<Item>();
            
            foreach (ItemRate ir in _dropList)
            {
                if(ir.DropRate.Equals(1))
                {
                    drops.Add(ir.Item);
                }
                else
                {
                    int chance = rand.Next(0, 100);
                    //item drops
                    if(chance <= ir.DropRate * 100)
                    {
                        drops.Add(ir.Item);
                    }
                }
            }
            
            return drops;
        }
        
        public void Die()
        {
            List<Item> drops = this.Drop();
            //add the drops to the map
            //delete the instance of the monster and leave a corpse
        }
    }
    
    /// <summary>
    /// Pairs an item with a drop rate
    /// </summary>
    public struct ItemRate
    {
        public Item Item { get; }
        //drop rate should be between 0.0 and 1.0 where 1.0 is 100% chance of drop
        public float DropRate { get; }
        
        public ItemRate(Item item, float rate)
        {
            this.Item = item;
            this.DropRate = rate;
        }