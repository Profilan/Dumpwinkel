using System;
using Dumpwinkel.Logic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dumpwinkel.Logic.Test
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void GetSettings()
        {
            var rep = new SettingRepository();

            var settings = rep.GetById(1);

            
        }
    }
}
