using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Mappings
{
    public class ScanMap : ClassMap<Scan>
    {
        public ScanMap()
        {
            Table("Scans");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Timestamp);
            Map(x => x.Status);

            References(x => x.Registration).Column("RegistrationId").Cascade.SaveUpdate();
        }
    }
}
