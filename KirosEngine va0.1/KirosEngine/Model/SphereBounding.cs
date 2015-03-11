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

        protected int _sphereSections; //how many sections to calculate for the sphere
        protected int _sphereRings; //how many rings to calculate for the sphere

        public float Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
                this.CalculateVerts();
            }
        }

        public SphereBounding(Vector3 position, float radius) : base(position)
        {
            _radius = radius;
        }
    }
}
