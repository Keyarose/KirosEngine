using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SlimDX;

namespace KirosProject.Enviroment
{
    class SkySphere
    {
        protected string _sphereID;

        protected string _worldID; //the id of the world the sphere is for

        protected List<StarData> _starList; //list of stars to be drawn on the sphere

        public SkySphere(string sphereID, string worldID)
        {
            _starList = new List<StarData>();
        }

        /// <summary>
        /// Constructor to create a skysphere from a xml doc
        /// </summary>
        /// <param name="xml">The xml doc to be used</param>
        public SkySphere(XDocument xml)
        {
            _starList = new List<StarData>(); //the list size should be retrieved from the doc and initialized
        }

        /// <summary>
        /// Constructor to create a skysphere from a file
        /// </summary>
        /// <param name="fileName">The name of the file to be used</param>
        public SkySphere(string fileName)
        {
            _starList = new List<StarData>();
        }

        /// <summary>
        /// Draw the skysphere using its data.
        /// </summary>
        public void Draw()
        {

        }
    }

    /// <summary>
    /// Define the data for a star to be drawn on the sphere
    /// </summary>
    struct StarData
    {
        public Vector2 _locPoint;
        public Color3 _pointColor;

        public StarData(Vector2 point, Color3 color)
        {
            _locPoint = point;
            _pointColor = color;
        }
    }
}
