using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirosPhysics
{
    public class Core
    {
    }

    [Flags]
    public enum ModuleFlags
    {
        Thermal = 1,
        Gravity = 2,
        Fluid = 4,
        Ragdoll = 8
    }
}
