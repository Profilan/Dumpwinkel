using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;

namespace Dumpwinkel.Logic.MappingsId
{
    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("Events");

            Id(x => x.Id).GeneratedBy.Guid();

            Component(x => x.TimeRange, m =>
            {
                m.Map(x => x.Start).Column("StartDate");
                m.Map(x => x.End).Column("EndDate");
            });

            Map(x => x.MaximumNumberOfVisitors);
            Map(x => x.PublishUp);
            Map(x => x.Tolerance);

            References(x => x.Dumpstore).Column("DumpstoreId").Cascade.SaveUpdate();
            References(x => x.Theme).Column("ThemeId").Cascade.SaveUpdate();
        }
    }
}
