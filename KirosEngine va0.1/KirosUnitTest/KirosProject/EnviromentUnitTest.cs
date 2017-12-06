using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KirosProject.Enviroment;

namespace KirosUnitTest.KirosProject
{
    [TestClass]
    class EnviromentUnitTest
    {
        [TestMethod]
        public void TempFactoryTests
        {
            TemperatureFactory fact1 = TemperatureFactory.Instance;
            
            assert( fact1.init(""));
        }
    }
}
