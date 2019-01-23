using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;

namespace Dumpwinkel.Logic.Mappings
{
    public class SettingMap : ClassMap<Setting>
    {
        public SettingMap()
        {
            Table("Settings");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Title);
            Map(x => x.TitleColor);
            Map(x => x.TitleSize);
            Map(x => x.IntroText);
            Map(x => x.IntroTextColor);
            Map(x => x.IntroTextSize);
            Map(x => x.BackgroundImageUrl);
        }
    }
}
