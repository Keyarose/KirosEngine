using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KirosEngine.EngineConsole
{
    /// <summary>
    /// Monitors the amount of available ram
    /// </summary>
    class RamMonitor : IDisposable
    {
        private PerformanceCounter _ramCounter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RamMonitor()
        {
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            _ramCounter.NextValue();
        }

        /// <summary>
        /// Get the amount of available ram in MB
        /// </summary>
        /// <returns>Returns the ram as a string</returns>
        public string GetAvailableRam()
        {
            return _ramCounter.NextValue() + "MB";
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            _ramCounter.Dispose();
        }
    }
}
