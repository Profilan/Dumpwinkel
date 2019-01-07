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
        public void CreateSlots()
        {
            var dumpstoreRep = new DumpstoreRepository();
            var eventRep = new EventRepository();

            var dumpstore = dumpstoreRep.GetById(new Guid("B980E94C-4436-4966-B291-2B377080E6E3"));

            var startDate = DateTime.Parse("2019-01-10 08:30:00");
            var endDate = DateTime.Parse("2019-01-10 18:00:00");

            var maxPersonsPerHour = 100;

            var events = Event.CreateRange(dumpstore, startDate, endDate, 30, maxPersonsPerHour);
            
            foreach (var newEvent in events)
            {
                eventRep.Insert(newEvent);
            }
        }

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

            var total = rep.getMaxPersonsByDate(DateTime.Parse("2018-12-27"));
        }

        [TestMethod]
        public void GetRegistration()
        {
            var rep = new RegistrationRepository();

            var item = rep.GetById(new Guid("F9B42485-1134-4C44-84D8-3A3D25219D7B"));
        }
    }
}
