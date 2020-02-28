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
    public class VisitorRepository : Profilan.SharedKernel.IRepository<Visitor, Guid>
    {
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Visitor GetById(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Visitor>(id);

                    return item;
                }
            }
        }

        public Visitor GetByEmail(string email)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var item = session.QueryOver<Visitor>()
                    .Where(x => x.Email == email)
                    .SingleOrDefault<Visitor>();

                return item;
            }
        }

        public void Insert(Visitor entity)
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

        public IEnumerable<Visitor> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Visitor> List()
        {
            throw new NotImplementedException();
        }

        public void Update(Visitor entity)
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
