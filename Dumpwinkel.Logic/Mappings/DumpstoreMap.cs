using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Mappings
{
    public class DumpstoreMap : ClassMap<Dumpstore>
    {
        public DumpstoreMap()
        {
            Table("Dumpstores");

            Id(x => x.Id).GeneratedBy.Guid();

            Map(x => x.Address);
            Map(x => x.Postcode);
            Map(x => x.City);
            Map(x => x.Geolocation);
        }
    }
}
