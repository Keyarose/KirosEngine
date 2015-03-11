using KirosEngine

namespace KirosProject
{
    /// <summary>
    /// A table which stores a value using two keys
    /// </summary>
    public class Table<T1, T2, T3>
    {
        protected Dictionary<string, Dictionary<string, T3>> _dictionary;
        
        /// <summary>
        /// Basic constructor
        /// </summary>
        public Table<T1, T2, T3>()
        {
            _dictionary = new Dictionary<string, Dictionary<string, T3>>();
        }
        
        /// <summary>
        /// Add a value to the table
        /// </summary>
        /// <params name="key1">The first level key for the table</params>
        /// <params name="key2">The second level key for the table</params>
        /// <params name="val">The value to be added</params>
        /// <returns>True if successful, false otherwise</returns>
        public bool Add(T1 key1, T2 key2, T3 val)
        {
            if(_dictionary.ContainsKey(key1.ToString()))
            {
                if(_dictionary[key1.ToString()].ContainsKey(key2.ToString()))
                {
                    //key already in use
                    ErrorLogger.Write(String.Format("Failed to add value: {0} to table: {1}. \n Key: {2} in use", val, this, key2));
                    return false;
                }
                else
                {
                    _dictionary[key1.ToString()].Add(key2.ToString(), val);
                    return true;
                }
            }
            else
            {
                _dictionary.Add(key1.ToString(), new Dictionary<string, T3>());
                _dictionary[key1.ToString()].Add(key2.ToString(), val);
                return true;
            }
        }
        
        /// <summary>
        /// Get the value for the given keys
        /// </summary>
        /// <params name="key1">The first level key to use</params>
        /// <params name="key2">The second level key to use</params>
        /// <returns>The value for the given keys, or null if not found</returns>
        public T3 GetValue(T1 key1, T2 key2)
        {
            if(_dictionary.ContainsKey(key1.ToString()))
            {
                if(_dictionary[key1.ToString()].ContainsKey(key2.ToString()))
                {
                    return _dictionary[key1.ToString()][key2.ToString()];
                }
                else
                {
                    //key not in use
                    ErrorLogger.Write(String.Format("Failed to get value from table: {0}. Key: {1} not in use.", this, key2));
                    return null;
                }
            }
            else
            {
                //key not in use
                ErrorLogger.Write(String.Format("Failed to get value from table: {0}. Key: {1} not in use.", this, key1));
                return null;
            }
        }
        
        /// <summary>
        /// Change the value for the given keys, does not add the value if it fails
        /// </summary>
        /// <param name="key1">The first level key to use</param>
        /// <param name="key2">The second level key to use</param>
        /// <param name="val"></param>
        /// <returns>Returns true if successful, false otherwise</returns>
        public bool ChangeValue(T1 key1, T2 key2, T3 val)
        {
            if(_dictionary.ContainsKey(key1.ToString()))
            {
                if(_dictionary[key1.ToString()].ContainsKey(key2.ToString()))
                {
                    _dictionary[key1.ToString()][key2.ToString()] = val;
                    return true;
                }
                else
                {
                    //key 2 not in use
                    ErrorLogger.Write(String.Format("Failed to change to value: {0} in table: {1} for key: {2}", val, this, key2));
                    return false;
                }
            }
            else
            {
                //key 1 not in use
                ErrorLogger.Write(String.Format("Failed to change to value: {0} in table: {1} for key: {2}", val, this, key1));
                return false;
            }
        }
        
        /// <summary>
        /// Removes the value for the given key set, logs an error if it fails
        /// </summary>
        /// <param name="key1">The first level key to use</param>
        /// <param name="key2">The second level key to use</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool RemoveValue(T1 key1, T2 key2)
        {
            if(_dictionary.ContainsKey(key1.ToString()))
            {
                if(_dictionary[key1.ToString()].ContainsKey(key2.ToString()))
                {
                    _dictionary[key1.ToString()].Remove(key2.ToString());
                    return true;
                }
                else
                {
                    //2nd key not in use
                    ErrorLogger.Write(String.Format("Failed to remove value from table: {0}. Key: {1} not in use.", this, key2));
                    return false;
                }
            }
            else
            {
                //first key not in use
                ErrorLogger.Write(String.Format("Failed to remove value from table: {0}. Key: {1} not in use.", this, key1));
                return false;
            }
        }
        
        /// <summary>
        /// Remove the given value from the table, logs an error if it fails
        /// </summary>
        /// <param name="val">The value to remove</param>
        /// <returns>True if successful false if it fails</returns>
        public bool RemoveValue(T3 val)
        {
            Dictionary<string, Dictionary<string, T3>>.ValueCollection dictCollect = _dictionary.Values;
            
            foreach(Dictionary<string, T3> d in dictCollect)
            {
                if(dictCollect.ContainsValue(val))
                {
                    dictCollect.Remove(val);
                    return true;
                }
            }
            
            ErrorLogger.Write(String.Format("Failed to remove value from table: {0}. Value: {1} is not in the table.", this, val));
            
            return false;
        }
    } 
}