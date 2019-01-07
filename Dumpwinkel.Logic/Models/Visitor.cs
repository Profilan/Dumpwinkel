using Profilan.SharedKernel;
using System;

namespace Dumpwinkel.Logic.Models
{
    public class Visitor : Entity<Guid>
    {
        public virtual string Name { get; protected set; }
        public virtual string City { get; protected set; }
        public virtual string Email { get; protected set; }

        protected Visitor()
        {

        }

        public Visitor(Guid id) : base(id)
        {

        }

        public static Visitor Create(string name,
            string city,
            string email)
        {
            Guard.ForNullOrEmpty(name, "name");
            Guard.ForNullOrEmpty(city, "city");
            Guard.ForNullOrEmpty(email, "email");

            var visitor = new Visitor(Guid.NewGuid());
            visitor.Name = name;
            visitor.City = city;
            visitor.Email = email;
            return visitor;
        }
    }
}