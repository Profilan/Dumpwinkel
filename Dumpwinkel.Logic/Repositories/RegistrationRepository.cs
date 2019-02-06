using Dumpwinkel.Logic.Data;
using Dumpwinkel.Logic.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

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

        public IEnumerable<Registration> List(string sortOrder, string searchString, int pageSize, int pageNumber, DateTime startDate, DateTime endDate, string eventId = null, string state = "all")
        {

            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from r in session.Query<Registration>()
                            select r;

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(r => r.Visitor.Name.Contains(searchString)
                                           || r.Visitor.Postcode.Contains(searchString)
                                           || r.Visitor.City.Contains(searchString)
                                           || r.Visitor.Email.Contains(searchString));
                }

                query = query.Where(r => r.Event.TimeRange.Start >= startDate && r.Event.TimeRange.Start <= endDate);
                switch (state)
                {
                     case "visited":
                        query = query.Where(r => r.Visited == true);
                        break;
                    case "confirmed":
                        query = query.Where(r => r.Confirmed == true);
                        break;
                }

                if (!String.IsNullOrEmpty(eventId))
                {
                    query = query.Where(r => r.Event.Id == new Guid(eventId));
                }
                

                switch (sortOrder)
                {
                    case "postcode":
                        query = query.OrderByDescending(r => r.Visitor.Postcode);
                        break;
                    case "name":
                        query = query.OrderBy(r => r.Visitor.Name);
                        break;
                    case "email":
                        query = query.OrderBy(r => r.Visitor.Email);
                        break;
                    case "city":
                        query = query.OrderBy(r => r.Visitor.City);
                        break;
                    case "timerange":
                    default:
                        query = query.OrderByDescending(r => r.Event.TimeRange.Start);
                        break;
                }

                return query.ToPagedList(pageNumber, pageSize);
            }
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

        public int GetRegisteredCount(Event eventItem)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
               Registration registration = null;
                var total = session.QueryOver<Registration>(() => registration)
                    .Select(Projections.Sum<Registration>(x => x.NumberOfVisitors))
                    .Where(x => x.Confirmed == true)
                    .Where(x => x.Event == eventItem)

                    .UnderlyingCriteria.UniqueResult();

                if (total != null)
                {
                    return (int)total;
                }
                return 0;
            }
        }

        public int GetPendingCount(Event eventItem)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                Registration registration = null;
                var total = session.QueryOver<Registration>(() => registration)
                    .Select(Projections.Sum<Registration>(x => x.NumberOfVisitors))
                    .Where(x => x.Confirmed == false)
                    .Where(x => x.Event == eventItem)

                    .UnderlyingCriteria.UniqueResult();

                if (total != null)
                {
                    return (int)total;
                }
                return 0;
            }
        }
    }
}
