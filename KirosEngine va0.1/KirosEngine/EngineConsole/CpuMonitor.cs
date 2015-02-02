using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KirosEngine.EngineConsole
{
    /// <summary>
    /// Handles getting the cpu usage
    /// </summary>
    class CpuMonitor
    {
        private PerformanceCounter _cpuCounter;
        private string _lastValue;
        private long _callLimiter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CpuMonitor()
        {
            _cpuCounter = new PerformanceCounter()
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            _lastValue = _cpuCounter.NextValue() + "%";
            _callLimiter = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Get the current CPU Usage
        /// </summary>
        /// <returns></returns>
        public string GetCpuUsage()
        {
            if(DateTime.Now.Ticks - _callLimiter >= TimeSpan.TicksPerSecond)
            {
                _lastValue = _cpuCounter.NextValue() + "%";
                _callLimiter = DateTime.Now.Ticks;
            }

            return _lastValue;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            _cpuCounter.Dispose();
        }
    }
}
