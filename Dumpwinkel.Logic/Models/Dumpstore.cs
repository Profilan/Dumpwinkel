using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Models
{
    public class Dumpstore : Entity<Guid>
    {
        public virtual string Address { get; protected set; }
        public virtual string Postcode { get; protected set; }
        public virtual string City { get; protected set; }
        public virtual string Geolocation { get; protected set; }

        protected Dumpstore()
        {

        }

        public Dumpstore(Guid id) : base(id)
        {

        }

        public static Dumpstore Create(string address,
            string postcode,
            string city,
            string geolocation)
        {
            Guard.ForNullOrEmpty(address, "address");
            Guard.ForNullOrEmpty(postcode, "postcode");
            Guard.ForNullOrEmpty(city, "city");

            var dumpstore = new Dumpstore(Guid.NewGuid());
            dumpstore.Address = address;
            dumpstore.Postcode = postcode;
            dumpstore.City = city;
            dumpstore.Geolocation = geolocation;

            return dumpstore;
        }
    }
}
