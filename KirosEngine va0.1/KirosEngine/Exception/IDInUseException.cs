using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Exception
{
    /// <summary>
    /// Represents an error thrown when an id is given that is already in use
    /// </summary>
    class IDInUseException : KirosException
    {
        //the id that caused the exception
        private string _id;
        //owner of the id
        private object _owner;
        //the manager that discovered the id was already in use
        private object _manager;

        public IDInUseException(string message, string id, object owner, object manager) : base(message)
        {
            _id = id;
            _owner = owner;
            _manager = manager;
        }

        public override string ToString()
        {
            string result = "IDInUseException: " + _manager + " attempted to register " + _owner + " with id: " + _id;

            return result;
        }
    }
}
