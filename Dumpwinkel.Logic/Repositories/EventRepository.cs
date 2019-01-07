using Dumpwinkel.Logic.Data;
using Dumpwinkel.Logic.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Repositories
{
    public class EventRepository : Profilan.SharedKernel.IRepository<Event, Guid>
    {
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Event GetById(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Event>(id);

                    return item;
                }
            }
        }
        
        

        public void Insert(Event entity)
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

        public IEnumerable<Event> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> List()
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from x in session.Query<Event>()
                            select x;

                query = query.OrderBy(x => x.TimeRange.Start);

                return query.ToList();
            }
        }

        public int GetMaxPersonsByDate(DateTime date)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                Event singleEvent = null;
                var total = session.QueryOver<Event>(() => singleEvent)
                    .Select(Projections.Sum<Event>(x => x.MaximumNumberOfVisitors))
                    .Where(x => x.TimeRange.Start.Date == date.Date)
                    .UnderlyingCriteria.UniqueResult();

                return Convert.ToInt32(total);
            }
        }

        public IEnumerable<Event> ListByDate(DateTime date)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from x in session.Query<Event>()
                            select x;

                query = query.OrderBy(x => x.TimeRange.Start);
                query = query.Where(x => x.TimeRange.Start.Date == date);

                return query.ToList();
            }
        }

        public void Update(Event entity)
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
