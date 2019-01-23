using Dumpwinkel.Logic.Data;
using Dumpwinkel.Logic.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Repositories
{
    public class RegistrationRepository : Profilan.SharedKernel.IRepository<Registration, Guid>
    {
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Registration GetById(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Registration>(id);

                    return item;
                }
            }
        }

        public IEnumerable<Registration> GetByEvent(Event eventItem)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = session.Query<Registration>()
                    .Where(x =>x.Event == eventItem);

                return query.ToList();
            }
        }

        public IEnumerable<Registration> GetByVisitorAndEvent(Visitor visitor, Event eventItem)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = session.Query<Registration>()
                    .Where(x => x.Visitor == visitor && x.Event == eventItem);

                return query.ToList();
            }
        }

        public void Insert(Registration entity)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Registration> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Registration> List()
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from x in session.Query<Registration>()
                            select x;

                query = query.OrderByDescending(x => x.Created);

                return query.ToList();
            }
        }

        public void Update(Registration entity)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
