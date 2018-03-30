using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KirosProject.Enviroment;

namespace KirosUnitTest.KirosProject
{
    [TestClass]
    class EnviromentUnitTest
    {
        [TestMethod]
        public void TempFactoryTests()
        {
            TemperatureFactory fact1 = TemperatureFactory.Instance;
            Assert.IsTrue( fact1.init(""), "Temperature Factory failed to initialize from string.");
        }
    }
}
