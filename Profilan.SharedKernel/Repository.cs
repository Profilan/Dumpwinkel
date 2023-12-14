using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Profilan.SharedKernel
{
    public abstract class Repository<T, TId>
        where T : Entity<TId>
    {
        public T GetById(TId id)
        {
            using (ISession session = SessionFactory.GetNewSession("db1"))
            {
                return session.Get<T>(id);
            }
        }

        public void Save(T entity)
        {
            using (ISession session = SessionFactory.GetNewSession("db1"))
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public IEnumerable<T> List()
        {
            using (ISession session = SessionFactory.GetNewSession("db1"))
            {
                var query = from l in session.Query<T>()
                            select l;

                return query.ToList();
            }
        }
    }
}
