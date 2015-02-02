using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Exception
{
    /// <summary>
    /// Abstract custom exception for the engine
    /// high level wrapper that all custom exceptions should inherit from
    /// </summary>
    [Serializable]
    abstract class KirosException : System.Exception
    {
        public KirosException(string message) : base(message) { }
    }
}
