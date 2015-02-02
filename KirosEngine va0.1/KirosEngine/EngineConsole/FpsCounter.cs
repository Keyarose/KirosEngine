using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine.EngineConsole
{
    /// <summary>
    /// Tracks the engine's fps
    /// </summary>
    class FpsCounter
    {
        private int _fps, _count;
        private long _startTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FpsCounter()
        {
            _fps = 0;
            _count = 0;
            _startTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Update the fps count
        /// </summary>
        public void Update()
        {
            _count++;

            if(DateTime.Now.Ticks - _startTime >= TimeSpan.TicksPerSecond)
            {
                _fps = _count;
                _count = 0;

                _startTime = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Return the fps count
        /// </summary>
        /// <returns>Returns the fps as an int value</returns>
        public int GetFps()
        {
            return _fps;
        }
    }
}
