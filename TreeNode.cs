namespace KirosProject
{
    public class TreeNode
    {
        private TreeNode _parentNode;
        private List<TreeNode> _childNodes;
        private Vector3 _location;
        private Vector3 _up;
        private float _length;
        private float _width;
        
        public int ChildNodeCount
        {
            get
            {
                return _childNodes.Count;
            }
        }
        
        public List<TreeNode>.Enumerator ChildNodeEnumerator
        {
            get
            {
                return _childNodes.GetEnumerator();
            }
        }
        
        /// <summary>
        /// Base constructor for the tree node
        /// </summary>
        public TreeNode(TreeNode parent, Vector3 location, Vector3 up, float length, float width)
        {
            _parentNode = parent;
            _location = location;
            _up = up;
            _length = length;
            _width = width;
            
            _childNodes = new List<TreeNode>();
        }
        
        /// <summary>
        /// Constructor for loading a node that was not just generated
        /// </summary>
        public TreeNode(TreeNode parent, Vector3 location, Vector3 up, float length, float width, params TreeNode[] children)
        {
            _parentNode = parent;
            _location = location;
            _up = up;
            _length = length;
            _width = width;
            _childNodes = new List<TreeNode>();
            _childNodes.Add(children);
        }
        
        /// <summary>
        /// Generate a node using the given location as a basis
        /// </summary>
        public static TreeNode GenNode(Vector3 location)
        {
            //gen the up direction, length, and width
        }
        
        //age the tree node based on the tree type's growth rate
        public void Age(float deltaAge)
        {
            //increase width, maybe length
        }
    }
}