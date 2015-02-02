namespace KirosProject
{
    public class Tree
    {
        private TreeBase _base;
        private List<TreeNode> _treeNodes;
        private List<RootNode> _rootNodes;
        
        private Texture _leafTexture;
        private Texture _barkTexture;
        private Texture _grainTexture;
        private int _maxChildNodes;
        
        public Tree(XElement xml)
        {
            this.Parse(xml);
        }
        
        public Tree(Texture leaf, Texture bark, Texture grain, Vector3 location)
        {
            this._leafTexure = leaf;
            this._barkTexture = bark;
            this._grainTexture = grain;
            
            this._base = new TreeBase(location);
        }
        
        //perform growth on the tree
        private Grow()
        {
            if(this._base.FirstTreeNode == null)
            {
                this._base.FirstTreeNode = TreeNode.GenNode(this._base.Location);
            }
            else
            {
                TreeNode currentNode = this._base.FirstTreeNode;
                if(currentNode.ChildNodeCount == _maxChildNodes)
                {
                    //age and transverse each child node
                    currentNode.Age();
                }
            }
        }
    }
}