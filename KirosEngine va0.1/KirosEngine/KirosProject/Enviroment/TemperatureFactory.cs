using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KirosProject.Enviroment
{
    /// <summary>
    /// Manages the creation of temperature measurement units, the creation of Temperature objects, and the conversion between units.
    /// </summary>
    class TemperatureFactory
    {
        protected List<TemperatureUnit> _unitList;
        protected TemperatureFactory _instance; //shouldn't be a need for more than one

        /// <summary>
        /// Public accessor for the Singleton
        /// </summary>
        public TemperatureFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TemperatureFactory();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Singleton Constructor
        /// </summary>
        /// <remarks>O(1) operation</remarks>
        protected TemperatureFactory()
        {
            _unitList = new List<TemperatureUnit>();//O(1)
        }

        /// <summary>
        /// Initialize the factory using the given file.
        /// </summary>
        /// <param name="fileName">The file to be used.</param>
        /// <returns>True if successfully executed, false if an error occurs.</returns>
        public bool init(string fileName)
        {
            return true;
        }

        /// <summary>
        /// ReInitialize the factory by clearing the unit list and reloading from the given file.
        /// </summary>
        /// <param name="fileName">The file to be used.</param>
        /// <returns>True if successfully executed, false if an error occurs.</returns>
        public bool reInit(string fileName)
        {
            return true;
        }

        /// <summary>
        /// Initialize the factory using the given xml document.
        /// </summary>
        /// <param name="xml">The xml document to be used.</param>
        /// <returns>True if successfully executed, false if an error occurs.</returns>
        public bool init(XDocument xml)
        {
            return true;
        }

        /// <summary>
        /// ReInitialize the factory using the given xml document.
        /// </summary>
        /// <param name="xml">The xml document to be used.</param>
        /// <returns>True if successfully executed, false if an error occurs.</returns>
        public bool reInit(XDocument xml)
        {
            return true;
        }

        /// <summary>
        /// Add a new temperature measurement unit to the factory.
        /// </summary>
        /// <param name="name">The name of the unit.</param>
        /// <param name="cf">The conversion factor between the new unit and Kelvin.</param>
        /// <remarks>O(1) if _unitList is less than capacity, O(n) if at capacity where n = capacity.</remarks>
        /// <returns>True if successful.</returns>
        public bool addUnit(string name, float cf)
        {
            _unitList.Add(new TemperatureUnit(name, cf));
            return true;
        }
    }

    /// <summary>
    /// Struct which defines a temperature measurement unit
    /// </summary>
    public struct TemperatureUnit
    {
        private string _name;
        private float _conversionFactor;

        public TemperatureUnit(string name, float cf)
        {
            _name = name;
            _conversionFactor = cf;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public float ConversionFactor
        {
            get
            {
                return _conversionFactor;
            }
        }
    }
}
