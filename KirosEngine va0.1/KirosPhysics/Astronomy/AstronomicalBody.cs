using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace KirosPhysics.Astronomy
{
    abstract class AstronomicalBody
    {
        protected Universe _universe;
        protected AstronomicalBody _parentBody; //what it orbits

        protected float _orbitRadiusAverage = 0.0f;
        protected float _orbitEccentricity;
        protected float _currentOrbitRadius; //internal use only?
        protected float _currentOrbitAngle; //angle from closest approach
        protected float _orbitRadiusMax;
        protected float _orbitRadiusMin;
        protected float _orbitPeriod;
        protected float _orbitVelocity;
        protected float _semiMajoralAxis;
        protected float _semiLatus;

        protected Vector3 _velocity;
        protected float _mass = 0.0f;

        #region Accessors
        /// <summary>
        /// Public accessor for the astronomical body's mass
        /// </summary>
        public float Mass
        {
            get
            {
                return _mass;
            }
        }
        /// <summary>
        /// Public accessor for the astronomical body's average orbital radius
        /// </summary>
        public float OrbitRadiusAverage
        {
            get
            {
                return _orbitRadiusAverage;
            }
            set
            {
                _orbitRadiusAverage = value;
            }
        }

        /// <summary>
        /// Public accessor for the astronomical body's orbital eccentricity
        /// </summary>
        public float OrbitEccentricity
        {
            get
            {
                return _orbitEccentricity;
            }
            set
            {
                _orbitEccentricity = value;
            }
        }

        /// <summary>
        /// Accessor for the astronomical body's maximum orbital radius
        /// </summary>
        public float OrbitRadiusMax
        {
            get
            {
                return _orbitRadiusMax;
            }
            set
            {
                _orbitRadiusMax = value;
            }
        }

        /// <summary>
        /// Accessor for the astronomical body's minimum orbital radius
        /// </summary>
        public float OrbitRadiusMin
        {
            get
            {
                return _orbitRadiusMin;
            }
            set
            {
                _orbitRadiusMin = value;
            }
        }

        /// <summary>
        /// Accessor for the astronomical body's orbital velocity
        /// Ref: http://en.wikipedia.org/wiki/Elliptic_orbit
        /// </summary>
        public float OrbitalVelocity
        {
            get
            {
                return _orbitVelocity;
            }
            set
            {
                _orbitVelocity = value;
            }
        }

        /// <summary>
        /// Accessor for the astronomical body's orbital period
        /// </summary>
        public float OrbitPeriod
        {
            get
            {
                return _orbitPeriod;
            }
            set
            {
                _orbitPeriod = value;
            }
        }
        #endregion

        #region Constructors
        protected AstronomicalBody()
        {

        }

        protected AstronomicalBody(float averageOrbitalRadius)
        {
            _orbitRadiusAverage = averageOrbitalRadius;
        }
        
        protected AstronomicalBody(float averageOrbitalRadius, AstronomicalBody parentBody)
        {
            _orbitRadiusAverage = averageOrbitalRadius;
            _parentBody = parentBody;
        }
        
        protected AstronomicalBody(float averageOrbitalRadius, AstronomicalBody parentBody, Universe universe)
        {
            _orbitRadiusAverage = averageOrbitalRadius;
            _parentBody = parentBody;
            _universe = universe;
        }
        #endregion

        /// <summary>
        /// Calculate the gravitational force for any object on the body
        /// </summary>
        /// <returns>Returns the force on an object on the body</returns>
        public float GravitationalForce()
        {
            return (float)(_universe.GravitationalConstant * _mass);
        }

        /// <summary>
        /// Calculate the gravitational force between this body and the given mass
        /// </summary>
        /// <param name="secondMass">The mass of the second body</param>
        /// <param name="distance">The distance between the two bodies</param>
        /// <returns>Returns the force between the two bodies as a floating point number</returns>
        public float GravitationalForce(float secondMass, float distance)
        {
            return (float)((_universe.GravitationalConstant * _mass * secondMass) / Math.Pow(distance, 2));
        }

        protected void Calculate()
        {
            //calc orbit radius or eccentricity
            if(_orbitEccentricity != null)
            {
                _orbitRadiusMax = _orbitRadiusAverage * (1 + _orbitEccentricity);
                _orbitRadiusMin = _orbitRadiusAverage * (1 - _orbitEccentricity);
            }
            else if(_orbitRadiusMax != null)
            {
                _orbitEccentricity = (_orbitRadiusMax / _orbitRadiusAverage) - 1;
            }
            else if(_orbitRadiusMin != null)
            {
                _orbitEccentricity = -((_orbitRadiusMin / _orbitRadiusAverage) - 1);
            }

            //calc semiLatus
            if(_orbitEccentricity != null && _orbitRadiusMin != null)
            {
                _semiLatus = _orbitRadiusMin * (1 + _orbitEccentricity);
            }
            
            //calc semi-majoral axis, radius min, or radius max
            if(_orbitRadiusMax != null && _orbitRadiusMin != null)
            {
                _semiMajoralAxis = (_orbitRadiusMax + _orbitRadiusMin) / 2.0f;
            }
            else if(_semiMajoralAxis != null && _orbitRadiusMax != null)
            {
                _orbitRadiusMin = (_semiMajoralAxis * 2.0f) - _orbitRadiusMax;
            }
            else if(_semiMajoralAxis != null && _orbitRadiusMin != null)
            {
                _orbitRadiusMax = (_semiMajoralAxis * 2.0f) - _orbitRadiusMin;
            }
            
            //calc orbit period or semi-majoral axis
            if(_semiMajoralAxis != null && _mass != null)
            {
                _orbitPeriod = (float)(2.0f * Math.PI * Math.Sqrt(Math.Pow(_semiMajoralAxis, 3) / _mass * _universe.GravitationalConstant));
            }
            else if(_orbitPeriod != null && _mass != null)
            {
                _semiMajoralAxis = (float)(Math.Pow(Math.Pow(_orbitPeriod / (2.0f * Math.PI), 2) * _mass * _universe.GravitationalConstant, 1.0f / 3.0f));
            }

            //calc current orbit radius
            if(_semiLatus != null && _orbitEccentricity != null)
            {
                _currentOrbitRadius = _semiLatus / (1 + _orbitEccentricity * _currentOrbitAngle);
            }

            //calc orbit velocity
            if(_mass != null && _semiMajoralAxis != null)
            {
                _orbitVelocity = (float)Math.Sqrt((double)(_mass * _universe.GravitationalConstant * (2.0f / _currentOrbitRadius - 1.0f / _semiMajoralAxis)));
            }
        }
    }
}
