using Profilan.SharedKernel;

namespace Dumpwinkel.Logic.Models
{
    public class Setting : Entity<int>
    {
        public virtual string Title { get; set; }
        public virtual string TitleColor { get; set; }
        public virtual int TitleSize { get; set; }
        public virtual string IntroText { get; set; }
        public virtual string IntroTextColor { get; set; }
        public virtual int IntroTextSize { get; set; }
        public virtual string BackgroundImageUrl { get; set; }

        protected Setting()
        {

        }

        public Setting(int id) : base(id)
        {

        }
    }
}
