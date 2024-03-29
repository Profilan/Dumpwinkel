﻿using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;

namespace Dumpwinkel.Logic.Mappings
{
    public class RegistrationMap : ClassMap<Registration>
    {
        public RegistrationMap()
        {
            Table("Registrations");

            Id(x => x.Id).GeneratedBy.Guid();

            Map(x => x.Created);
            Map(x => x.Modified);
            Map(x => x.NumberOfVisitors);
            Map(x => x.Confirmed);
            Map(x => x.ConfirmationDate);
            Map(x => x.Visited);
            Map(x => x.RejectionReason).Nullable();
            Map(x => x.IPAddress).Nullable();
            // Added for cancellation possibility (1-10-2021 R.A. Soffner)
            Map(x => x.Cancelled).Nullable();

            References(x => x.Event).Column("EventId").LazyLoad().Not.Cascade.SaveUpdate();
            References(x => x.Visitor).Column("VisitorId").Cascade.SaveUpdate();

            HasMany(x => x.Scans)
                .Cascade.SaveUpdate()
                .KeyColumn("RegistrationId")
                .LazyLoad().Not
                .Inverse();
        }
    }
}
