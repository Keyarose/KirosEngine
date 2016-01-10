using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosEngine
{
    class GameTime
    {
        public TimeSpan TotalGameTime { get; set; }
        public TimeSpan ElapsedGameTime { get; set; }

        public GameTime()
        {
            TotalGameTime = TimeSpan.Zero;
            ElapsedGameTime = TimeSpan.Zero;
        }
    }
}
