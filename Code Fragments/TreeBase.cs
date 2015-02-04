namespace KirosProject
{
    public class TreeBase
    {
        private Vector3 _location;
        private TreeNode _firstNode;
        private RootNode _firstRoot;
        
        public TreeNode FirstTreeNode
        {
            get
            {
                return _firstNode;
            }
            set
            {
                _firstNode = value;
            }
        }
        
        public RootNode FirstRootNode
        {
            get
            {
                return _firstRoot;
            }
            set
            {
                _firstRoot = value;
            }
        }
        
        public Vector3 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        
        public TreeBase(Vector3 location)
        {
            _location = location;
        }
    }
}