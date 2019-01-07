using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Mappings
{
    public class VisitorMap : ClassMap<Visitor>
    {
        public VisitorMap()
        {
            Table("Visitors");

            Id(x => x.Id).GeneratedBy.Guid();

            Map(x => x.Name);
            Map(x => x.City);
            Map(x => x.Email);
        }
    }
}
