namespace KirosProject
{
    public class TerrainPart
    {
        protected List<TPVertex> _vertices;
        protected WorldMaterial _partMaterial;
        
        public struct TPVertex
        {
            float x, y, z;
            float nx, ny, nz;
            float time;
        }
        
        /// <summary>
        /// Basic constructor for TerrainPart
        /// </summary>
        /// <params name="material">The WorldMaterial which discribes the terrain part.</params>
        public TerrainPart(WorldMaterial material)
        {
            _partMaterial = material;
        }
        
        public TerrainPart(WorldMaterial material, IEnumerable<TPVertex> vertices)
        {
            _partMaterial = material;
            _vertices = vertices.ToList();
        }
    }
    
    public enum WorldMaterial
    {
        Dirt,
        Gravel,
        Stone,
        Snow,
        Ice
    }
}