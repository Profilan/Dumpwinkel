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

        [TestMethod]
        public void GetRegistration()
        {
            var rep = new RegistrationRepository();

            var registration = rep.GetById(new Guid("5CC38093-DB62-4FCD-8562-0F013C51FA0A"));
        }
    }
}
