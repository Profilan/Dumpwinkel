using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;

namespace Dumpwinkel.Logic.Mappings
{
    public class ThemeMap : ClassMap<Theme>
    {
        public ThemeMap()
        {
            Table("Themes");

            Id(x => x.Id).GeneratedBy.Guid();

            Map(x => x.Title);
            Map(x => x.Description);
        }
    }
}
