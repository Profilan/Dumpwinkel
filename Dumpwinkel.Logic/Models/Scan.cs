using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Models
{
    public class Scan : Entity<int>
    {
        public virtual DateTime Timestamp { get; set; }
        public virtual string Status { get; set; }

        public virtual Registration Registration { get; set; }

        public static Scan Create(DateTime timestamp, string status, Registration registration)
        {
            var newScan = new Scan();
            newScan.Timestamp = timestamp;
            newScan.Status = status;
            newScan.Registration = registration;

            return newScan;
        }
    }
}
