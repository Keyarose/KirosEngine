using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace KirosPhysics.Astronomy
{
    class Star : AstronomicalBody
    {
        protected SpectralClassification _classification;
        protected float _metallicity;

        //physical characteristics
        protected double _radius;
        protected double _density;
        protected float _surfaceGravity;
        protected float _escapeVelocity;
        protected float _temperature;
        protected float _luminosity;
        protected float _age;
        
        public SpectralClassification Classification
        {
            get
            {
                return _classification;
            }
        }
        
        public float Metallicity
        {
            get
            {
                return _metallicity;
            }
        }
        
        public double Radius
        {
            get
            {
                return _radius;
            }
        }
        
        public double Density
        {
            get
            {
                return _density;
            }
        }
        
        public float SurfaceGravity
        {
            get
            {
                return _surfaceGravity;
            }
        }
        
        public float EscapeVelocity
        {
            get
            {
                return _escapeVelocity;
            }
        }
        
        public float Temperature
        {
            get
            {
                return _temperature;
            }
        }
        
        public float Luminosity
        {
            get
            {
                return _luminosity;
            }
        }
        
        public float Age
        {
            get
            {
                return _age;
            }
        }
        
        #region Constructors
        public Star(Star star)
        {
            _mass = star.Mass;
            _radius = star.Radius;
            _density = star.Density;
            _temperature = star.Temperature;
            _luminosity = star.Luminosity;
            _age = star.Age;
            
            _classification = star.Classification;
            _metallicity = star.Metallicity;
        }
        
        public Star(float mass, double radius)
        {
            _mass = mass;
            _radius = radius;
        }
        
        public Star(float mass, double radius, double density)
        {
            _mass = mass;
            _radius = radius;
            _density = density;
        }
        
        public Star(float mass, double radius, double density, float temperature)
        {
            _mass = mass;
            _radius = radius;
            _density = density;
            _temperature = temperature;
        }
        
        public Star(float mass, double radius, double density, float temperature, float luminosity)
        {
            _mass = mass;
            _radius = radius;
            _density = density;
            _temperature = temperature;
            _luminosity = luminosity;
        }
        
        public Star(float mass, double radius, double density, float temperature, float luminosity, float age)
        {
            _mass = mass;
            _radius = radius;
            _density = density;
            _temperature = temperature;
            _luminosity = luminosity;
            _age = age;
        }
        #endregion
    }

    /// <summary>
    /// Defines a star's classification
    /// </summary>
    struct SpectralClassification
    {
        SpectralType _type;
        float _absoluteMagnitude;
        LuminosityClass _luminosity;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="type">The spectral type of the star</param>
        /// <param name="magnitude">The magnitude of the star</param>
        /// <param name="luminosity">The luminosity of the star</param>
        SpectralClassification(SpectralType type, float magnitude, LuminosityClass luminosity)
        {
            _type = type;
            _absoluteMagnitude = magnitude;
            _luminosity = luminosity;
        }
    }

    /// <summary>
    /// Defines the spectral types 
    /// </summary>
    enum SpectralType
    {
        O,
        B,
        A,
        F,
        G,
        K,
        M,
        L,
        T
    }

    /// <summary>
    /// Defines the different luminosity classes
    /// </summary>
    enum LuminosityClass
    {
        O,
        Ia,
        Ib,
        II,
        III,
        IV,
        V,
        VI,
        VII
    }
}
