using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Models
{
    public class Registration : Entity<Guid>
    {
        public virtual Visitor Visitor { get; protected set; }
        public virtual Event Event { get; protected set; }
        public virtual int NumberOfVisitors { get; protected set; }

        public virtual DateTime Created { get; protected set; }
        public virtual DateTime Modified { get; protected set; }

        public virtual bool Confirmed { get; set; }
        public virtual DateTime ConfirmationDate { get; set; }
        public virtual bool Visited { get; set; }

        public virtual IList<Scan> Scans { get; set; }

        #region Not Persisted
        public virtual int CreatedBy { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int State { get; set; }
        #endregion

        protected Registration()
        {

        }

        public Registration(Guid id) : base(id)
        {

        }

        public static Registration Create(Visitor visitor,
            Event eventItem,
            int numberOfVisitors,
            bool confirmed = false,
            bool visited = false)
        {
            Guard.ForNull(visitor, "visitor");
            Guard.ForNull(eventItem, "eventItem");

            var registration = new Registration(Guid.NewGuid());
            registration.Visitor = visitor;
            registration.Event = eventItem;
            registration.NumberOfVisitors = numberOfVisitors;
            registration.Created = DateTime.Now;
            registration.Modified = registration.Created;
            registration.Confirmed = confirmed;
            registration.ConfirmationDate = registration.Created;
            registration.Visited = visited;

            return registration;
        }
    }
}
