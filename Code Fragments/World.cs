namespace KirosProject
{
    //represents an entire world
    public class World
    {
        private string _name;
        private string _star;
        private string _universe;
        
        private float _area;
        private float _gravity;
        private Orbit _orbit;
        
        private List<WorldChunk> _chunks;
        
        public World(string name)
        {
            _name = name;
        }
        
        public World(XElement xml)
        {
            this.ParseXml(xml);
        }
        
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        public string Star
        {
            get
            {
                return _star;
            }
        }
        
        public string Universe
        {
            get
            {
                return _universe;
            }
        }
        
        //?
        public IEnumeratable GetChunks()
        {
            return _chunks.Enumerate();//?
        }
        
        //get a chunk by the chunk id
        public WorldChunk GetChunk(string chunkID)
        {
        
        }
        
        //get a chunk by the chunk coordinates
        public WorldChunk GetChunk(Vector2 chunkCoordinates)
        {
        
        }
        
        private void ParseXml(XElement xml)
        {
        
        }
    }
}