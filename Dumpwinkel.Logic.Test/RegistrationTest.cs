using System;
using Dumpwinkel.Logic.Models;
using Dumpwinkel.Logic.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dumpwinkel.Logic.Test
{
    [TestClass]
    public class RegistrationTest
    {
         [TestMethod]
        public void CreateDumpstore()
        {
            var rep = new DumpstoreRepository();
            var dumpstore = Dumpstore.Create("Zaadmarkt 25", "1681 PD", "Zwaagdijk", "52.7011083,5.1458609");

            rep.Insert(dumpstore);
        }

        [TestMethod]
        public void GetMaxPersonsByDate()
        {
            var rep = new EventRepository();

            var total = rep.GetMaxPersonsByDate(DateTime.Parse("2018-12-27"));
        }

        [TestMethod]
        public void GetRegistration()
        {
            var rep = new RegistrationRepository();

            var item = rep.GetById(new Guid("F9B42485-1134-4C44-84D8-3A3D25219D7B"));
        }
    }
}
