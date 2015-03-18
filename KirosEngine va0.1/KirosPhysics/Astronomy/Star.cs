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

        //Orbital characteristics
        protected double _orbitDistance;
        protected double _orbitVelocity;

        //physical characteristics
        protected double _radius;
        protected double _density;
        protected float _surfaceGravity;
        protected float _escapeVelocity;
        protected float _temperature;
        protected float _luminosity;
        protected float _age;
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
