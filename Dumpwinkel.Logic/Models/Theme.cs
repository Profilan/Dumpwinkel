using Profilan.SharedKernel;
using System;
using System.Collections.Generic;

namespace Dumpwinkel.Logic.Models
{
    public class Theme : Entity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

        public virtual IList<Event> Events { get; set; }

        protected Theme()
        {

        }

        public Theme(Guid id) : base(id)
        {

        }

        public static Theme Create(string title, string description)
        {
            var newTheme = new Theme(Guid.NewGuid());
            newTheme.Title = title;
            newTheme.Description = description;

            return newTheme;
        }
    }
}
