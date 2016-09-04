using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Events
{
    class Event
    {
        protected string _id;
        protected object _sender;
        protected List<object> _arguments;

        public Event(string eventID, object sender, List<object> eventArguments)
        {
            _id = eventID;
            _sender = sender;
            _arguments = eventArguments;
        }

        public string ID
        {
            get
            {
                return _id;
            }
        }

        public object Sender
        {
            get
            {
                return _sender;
            }
        }

        public List<object> Arguments
        {
            get
            {
                return _arguments;
            }
        }
    }
}
