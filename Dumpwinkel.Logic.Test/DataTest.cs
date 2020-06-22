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
        public void GetRegistrationTotal()
        {
            var rep = new RegistrationRepository();

            var currentDate = DateTime.Now;
            var start = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            var end = start.AddHours(24);

            // var total = rep.GetRegistrationTotal(start, end);
        }

        [TestMethod]
        public void GetRegistration()
        {
            var rep = new RegistrationRepository();

            var registration = rep.GetById(new Guid("1B7FC4C9-ECBB-426A-9263-D59449958313"));
        }

        [TestMethod]
        public void getEvents()
        {
            var rep = new EventRepository();

            var events = rep.ListByDate(new DateTime(2020, 6, 20));
        }

        [TestMethod]
        public void GetScansByRegistration()
        {
            var rep = new ScanRepository();

            var scans = rep.ListByRegistration(new Guid("1B7FC4C9-ECBB-426A-9263-D59449958313"));
        }

    }
}
