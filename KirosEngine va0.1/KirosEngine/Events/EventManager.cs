using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.Events
{
    class EventManager
    {
        private static EventManager instance;

        private Dictionary<string, List<Delegate>> _regesteredObservers;

        /// <summary>
        /// Default constructor
        /// </summary>
        private EventManager()
        {

        }

        public static EventManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }

        public bool RegisterObserver(string eventID, Delegate observer)
        {
            if(!_regesteredObservers.ContainsKey(eventID))
            {
                _regesteredObservers.Add(eventID, new List<Delegate>());
            }

            if(_regesteredObservers[eventID].Contains(observer))
            {
                return false;
            }

            _regesteredObservers[eventID].Add(observer);

            return true;
        }

        public bool RemoveObserver(string eventID, Delegate observer)
        {
            if(!_regesteredObservers.ContainsKey(eventID))
            {
                return false;
            }

            if(!_regesteredObservers[eventID].Contains(observer))
            {
                return false;
            }

            _regesteredObservers[eventID].Remove(observer);

            return true;
        }

        public bool RemoveObserverForAllEvents(Delegate observer)
        {
            foreach(KeyValuePair<string, List<Delegate>> ld in _regesteredObservers)
            {
                if(ld.Value.Contains(observer))
                {
                    ld.Value.Remove(observer);
                }
            }

            return true;
        }
    }
}
