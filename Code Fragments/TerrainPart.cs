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
        
        public List<TPVertex>.Enumerator GetVertices()
        {
            return _vertices.GetEnumerator();
        }
        
        public WorldMaterial GetWorldMaterial()
        {
            return _partMaterial;
        }
        
        public bool ShiftVerts(float deltaTime, Vector3 cameraDirection, Vector3 cameraPosition)
        {
            //use camera position and direction to find the verts closest to where the player is looking
            //then shift the verts by a dig factor along the direction vector
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