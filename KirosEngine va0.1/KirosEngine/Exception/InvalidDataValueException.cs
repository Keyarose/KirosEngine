using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Exception
{
    /// <summary>
    /// Represents an error thrown when a value read in from a data file is not the expected type
    /// </summary>
    class InvalidDataValueException : KirosException
    {
        //the type the value was expected to be
        private string _expectedType;
        //the type of value that was read in
        private string _actualType;
        //the value that was read in
        private object _value;

        public InvalidDataValueException(string message, string expectedType, string actualType, object value)
            : base(message)
        {
            _expectedType = expectedType;
            _actualType = actualType;
            _value = value;
        }

        public override string ToString()
        {
            string result = "InvalidDataValueException: Expected type " + _expectedType + " but recived type " + _actualType + " with value " + _value;
            return result;
        }
    }
}
