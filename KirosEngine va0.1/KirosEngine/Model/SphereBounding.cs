using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace KirosEngine.Model
{
    class SphereBounding : Bounding
    {
        protected float _radius;

        /// <summary>
        /// Public accessor for the sphere's radius
        /// </summary>
        public float Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="position">The position the bounding is at</param>
        /// <param name="radius">The radius of the bounding sphere</param>
        public SphereBounding(Vector3 position, float radius) : base(position)
        {
            _radius = radius;
        }

        /// <summary>
        /// Finds if the given point is within the bounds of the sphere
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Returns true if the point is within the sphere, false otherwise</returns>
        public override bool InsideBounds(Vector3 point)
        {
            if(Vector3.Distance(_position, point) < _radius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Unused for sphere as it is a simple bounds
        /// </summary>
        protected override void CalculateVerts()
        {
            //unused for sphere
            throw new NotImplementedException("This is not suppose to be called!");
        }
    }
}
