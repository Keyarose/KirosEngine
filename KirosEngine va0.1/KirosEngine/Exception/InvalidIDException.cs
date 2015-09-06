using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Exception
{
    /// <summary>
    /// Represents an error thrown when a supplied ID value 
    /// </summary>
    class InvaildIDException : KirosException
    {
        private string _idType; //texture, shader, scene, etc.
        private string _value;
        private object _sender;
        
        public InvaildIDException(string message, string type, string value, object sender) : base(message)
        {
            _idType = type;
            _value = value;
            _sender = sender;
        }
        
        public override string ToString()
        {
            string result = "InvalidIDException: ID Type " + _type + " with value " + _value + " was sent to  " _sender.ToString();
            return result;
        }
    }
}