using Dumpwinkel.Logic.Models;
using NHibernate;
using Dumpwinkel.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dumpwinkel.Logic.Repositories
{
    public class ThemeRepository : Profilan.SharedKernel.IRepository<Theme, Guid>
    {
        public void Delete(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var item = session.Load<Theme>(id);

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(item);
                    transaction.Commit();
                }
            }
        }

        public Theme GetById(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Theme>(id);

                    return item;
                }
            }
        }

        public void Insert(Theme entity)
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

        public IEnumerable<Theme> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Theme> List()
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from x in session.Query<Theme>()
                            select x;

                query = query.OrderBy(x => x.Title);

                return query.ToList();
            }
        }

        public void Update(Theme entity)
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
