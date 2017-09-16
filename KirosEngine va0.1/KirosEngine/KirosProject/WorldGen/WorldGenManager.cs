using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirosEngine;

namespace KirosProject.WorldGen
{
    class WorldGenManager
    {
        protected WorldGenManager _instance;

        public WorldGenManager Instance
        {
            get
            {
                if(_instance != null)
                {
                    return _instance;
                }
                else
                {
                    _instance = new WorldGenManager();
                    return _instance;
                }
            }
        }

        protected WorldGenManager()
        {

        }
    }
}
