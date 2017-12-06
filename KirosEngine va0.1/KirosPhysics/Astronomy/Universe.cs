using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosPhysics.Astronomy
{
    class Universe
    {
        protected float _gravitaionalConstant;
        protected string _name; //natural name
        protected string _id; //system identification

        public float GravitationalConstant
        {
            get
            {
                return _gravitaionalConstant;
            }
        }

        #region constructors
        /// <summary>
        /// Basic constructor for Universe
        /// </summary>
        /// <param name="id">Unique Id string</param>
        /// <param name="name">Natural name of the Universe</param>
        public Universe(string id, string name)
        {
            _id = id;
            _name = name;
        }

        /// <summary>
        /// Constructor for Universe including the gc
        /// </summary>
        /// <param name="id">Unique Id string</param>
        /// <param name="name">Natural name of the Universe</param>
        /// <param name="gc">Gravitational Constant of the Universe</param>
        public Universe(string id, string name, float gc)
        {
            _id = id;
            _name = name;
            _gravitaionalConstant = gc;
        }
        #endregion
    }
}
