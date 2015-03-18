using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using KirosEngine.Model;

namespace KirosEngine.Scene
{
    /// <summary>
    /// Top level abstract class for objects within a scene
    /// </summary>
    abstract class SceneNode
    {
        protected string _nodeID;

        protected Vector3 _position;
        protected Vector3 _scale;
        protected Vector3 _rotation;

        protected Bounding _bounds;

        protected SceneNode _parent;
        protected List<SceneNode> _children;

        #region Properties
        /// <summary>
        /// Public accessor for the node's position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Public accessor for the node's scale
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return _scale;
            }
        }

        /// <summary>
        /// Public accessor for the node's rotation
        /// </summary>
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
        }

        /// <summary>
        /// Public accessor for the node's parent
        /// </summary>
        public SceneNode Parent
        {
            get
            {
                return _parent;
            }
        }

        /// <summary>
        /// Public accessor for the node's children
        /// </summary>
        public List<SceneNode>.Enumerator Children
        {
            get
            {
                return _children.GetEnumerator();
            }
        }
        #endregion

        #region Setters
        public void SetPosition(Vector3 position)
        {
            this._position = position;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        public SceneNode(string id, Vector3 position)
        {
            _nodeID = id;
            _position = position;
            _scale = new Vector3(0.0f);
            _rotation = new Vector3(0.0f);
            _children = new List<SceneNode>();
        }

        /// <summary>
        /// Basic constructor with a parent node
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        /// <param name="parent">The parent node</param>
        public SceneNode(string id, Vector3 position, SceneNode parent)
        {
            _nodeID = id;
            _position = position;
            _scale = new Vector3(0.0f);
            _rotation = new Vector3(0.0f);
            _parent = parent;
            _children = new List<SceneNode>();
        }

        /// <summary>
        /// Basic constructor with a parent and children nodes
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        /// <param name="parent">The parent node</param>
        /// <param name="children">The child nodes</param>
        public SceneNode(string id, Vector3 position, SceneNode parent, params SceneNode[] children)
        {
            _nodeID = id;
            _position = position;
            _scale = new Vector3(0.0f);
            _rotation = new Vector3(0.0f);
            _parent = parent;
            _children = new List<SceneNode>(children);
        }

        /// <summary>
        /// Constructor with all transforms
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        /// <param name="scale">The scale of the node</param>
        /// <param name="rotation">The rotation of the node</param>
        public SceneNode(string id, Vector3 position, Vector3 scale, Vector3 rotation)
        {
            _nodeID = id;
            _position = position;
            _scale = scale;
            _rotation = rotation;
            _children = new List<SceneNode>();
        }

        /// <summary>
        /// Constructor with all transforms and a parent node
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        /// <param name="scale">The scale of the node</param>
        /// <param name="rotation">The rotation of the node</param>
        /// <param name="parent">The parent node</param>
        public SceneNode(string id, Vector3 position, Vector3 scale, Vector3 rotation, SceneNode parent)
        {
            _nodeID = id;
            _position = position;
            _scale = scale;
            _rotation = rotation;
            _parent = parent;
            _children = new List<SceneNode>();
        }

        /// <summary>
        /// Constructor with all transforms, a parent node, and child nodes
        /// </summary>
        /// <param name="id">Node identifier string</param>
        /// <param name="position">The position the node is at</param>
        /// <param name="scale">The scale of the node</param>
        /// <param name="rotation">The rotation of the node</param>
        /// <param name="parent">The parent node</param>
        /// <param name="children">The child nodes</param>
        public SceneNode(string id, Vector3 position, Vector3 scale, Vector3 rotation, SceneNode parent, params SceneNode[] children)
        {
            _nodeID = id;
            _position = position;
            _scale = scale;
            _rotation = rotation;
            _parent = parent;
            _children = new List<SceneNode>(children);
        }
        #endregion

        #region Transforms
        /// <summary>
        /// Translate the node from its current position by the given value
        /// </summary>
        /// <param name="translation">The value to translate the node by.</param>
        public void TranslateNode(Vector3 translation)
        {
            _position += translation;
        }

        /// <summary>
        /// Scale the node from its current size by the given value
        /// </summary>
        /// <param name="scale">The value to scale the node by.</param>
        public void ScaleNode(Vector3 scale)
        {
            _scale += scale;
        }

        /// <summary>
        /// Rotate the node from its current rotation by the given value
        /// </summary>
        /// <param name="rotation">The value to rotate by.</param>
        public void RotateNode(Vector3 rotation)
        {
            _rotation += rotation;
        }
        #endregion
    }
}
