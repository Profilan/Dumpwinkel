using Dumpwinkel.Logic.Models;
using FluentNHibernate.Mapping;
using Profilan.SharedKernel;

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
            Map(x => x.InfoText);
            Map(x => x.BackgroundImageUrl);
            Map(x => x.EmailDisclaimer);
            Map(x => x.Tolerance);

            Component(x => x.LegacyPeriod, m =>
            {
                m.Map(x => x.Amount).Column("LegacyAmount").Nullable();
                m.Map(x => x.Unit).Column("LegacyUnit").CustomType(typeof(Unit)).Nullable();
            });

            Map(x => x.LegacyText);
            Map(x => x.AlreadyText);
        }
    }
}
