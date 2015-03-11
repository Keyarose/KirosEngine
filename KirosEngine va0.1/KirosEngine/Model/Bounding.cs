using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace KirosEngine.Model
{
    abstract class Bounding
    {
        protected List<BoundsVertex> _vertices;

        protected Vector3 _position;

        /// <summary>
        /// Public accessor for the Bounding's position, should be the same as the node it is assigned to
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public Bounding(Vector3 position)
        {
            _position = position;
        }

        public abstract bool InsideBounds();
        protected abstract void CalculateVerts();
    }

    struct BoundsVertex
    {
        float x,y;
    }
}
